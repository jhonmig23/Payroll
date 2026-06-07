using FluentMigrator;

namespace Api.Migrations
{
    [Migration(202606070002)]
    public class AddComputePayAsyncProcedure : FluentMigrator.Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE PROCEDURE ComputePayAsync
                    @EmployeeId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @WorkingDays VARCHAR(10);
                    DECLARE @DailyRate DECIMAL(10, 2);
                    DECLARE @DateOfBirth DATE;

                    SELECT
                        @WorkingDays = WorkingDays,
                        @DailyRate = DailyRate,
                        @DateOfBirth = DateOfBirth
                    FROM Employees
                    WHERE Id = @EmployeeId;

                    IF @@ROWCOUNT = 0
                    BEGIN
                        -- Employee not found, return 0 or raise an error?
                        -- We'll return 0 for now, but the service layer throws KeyNotFoundException.
                        SELECT 0.0 AS ComputedPay;
                        RETURN;
                    END

                    DECLARE @NumberOfWorkingDays INT =
                        CASE @WorkingDays
                            WHEN 'MWF' THEN 3
                            WHEN 'TTHS' THEN 2
                            ELSE 0
                        END;

                    DECLARE @BasePay DECIMAL(10, 2) = @NumberOfWorkingDays * @DailyRate * 2;

                    DECLARE @IsBirthday BIT =
                        CASE
                            WHEN MONTH(GETDATE()) = MONTH(@DateOfBirth) AND DAY(GETDATE()) = DAY(@DateOfBirth) THEN 1
                            ELSE 0
                        END;

                    DECLARE @BirthdayBonus DECIMAL(10, 2) = CASE WHEN @IsBirthday = 1 THEN @DailyRate ELSE 0 END;

                    SELECT @BasePay + @BirthdayBonus AS ComputedPay;
                END;
            ");
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE ComputePayAsync;");
        }
    }
}