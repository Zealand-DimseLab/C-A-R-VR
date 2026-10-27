namespace cardataapi;
using System.Data;
using System.Configuration.Assemblies;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Collections;
using Microsoft.Data.SqlClient;
using CARDataLib;
public class CarDataMssqlRepository{
    private readonly string connectionString;
    public CarDataMssqlRepository(string connectionString)
    {
        this.connectionString = connectionString; 
    }
    public User InsertQuaterOne(User user)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    IF EXISTS (SELECT 1 FROM Users WHERE TestPersonNumber = @testpersonnumber)
                    UPDATE Users SET Name=@name, Gender=@gender, Age=@age, Consent=@consent,
                Q1=@q1, Q1Comment=@q1comment, Q2=@q2, Q3=@q3, Q4=@q4
                    WHERE TestPersonNumber=@testpersonnumber
                    ELSE
                    INSERT INTO Users (TestPersonNumber, Name, Gender, Age, Consent, Q1, Q1Comment, Q2, Q3, Q4)
                    VALUES (@testpersonnumber, @name, @gender, @age, @consent, @q1, @q1comment, @q2, @q3, @q4)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    HelperMethods.BindValueInt(command, "@testpersonnumber", user.TestPersonNumber);
                    HelperMethods.BindValueString(command, "@name", user.Name);
                    HelperMethods.BindValueString(command, "@gender", user.Gender);
                    HelperMethods.BindValueString(command, "@age", user.Age);
                    HelperMethods.BindValueInt(command, "@consent", user.Consent);
                    HelperMethods.BindValueString(command, "@q1", user.Q1);
                    HelperMethods.BindValueString(command, "@q1comment", user.Q1Comment);
                    HelperMethods.BindValueString(command, "@q2", user.Q2);
                    HelperMethods.BindValueString(command, "@q3", user.Q3);
                    HelperMethods.BindValueString(command, "@q4", user.Q4);
                    command.ExecuteNonQuery();
                }
            }
            return user;
        }
        catch (SqlException e)
        {
            throw new Exception(e.Message);
        }
    }

    public User InsertQuaterTwo(User user)
{
    try
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string sql = @"UPDATE Users SET Q5=@q5, Q6=@q6, Q7=@q7, Q8=@q8, Q9=@q9,
                                            Q10=@q10, Q11=@q11, Q12=@q12, Q13=@q13
                           WHERE TestPersonNumber=@testpersonnumber";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                HelperMethods.BindValueInt(command, "@testpersonnumber", user.TestPersonNumber);
                HelperMethods.BindValueString(command, "@q5", user.Q5);
                HelperMethods.BindValueString(command, "@q6", user.Q6);
                HelperMethods.BindValueString(command, "@q7", user.Q7);
                HelperMethods.BindValueString(command, "@q8", user.Q8);
                HelperMethods.BindValueString(command, "@q9", user.Q9);
                HelperMethods.BindValueString(command, "@q10", user.Q10);
                HelperMethods.BindValueString(command, "@q11", user.Q11);
                HelperMethods.BindValueString(command, "@q12", user.Q12);
                HelperMethods.BindValueString(command, "@q13", user.Q13);
                if (command.ExecuteNonQuery() == 0)
                    throw new Exception("Testperson findes ikke. Send quaterone først.");
            }
        }
        return user;
    }
    catch (SqlException e)
    {
        throw new Exception(e.Message);
    }
}
    public Notes InsertNotes(Notes notes)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = @"
    IF EXISTS (SELECT 1 FROM Notes WHERE UserId = @userId)
        UPDATE Notes SET ScenarioName1=@scenarioname1, ScenarioNote1=@scenarionotes1,
                         ScenarioName2=@scenarioname2, ScenarioNote2=@scenarionotes2,
                         ScenarioName3=@scenarioname3, ScenarioNote3=@scenarionotes3
        WHERE UserId=@userId
    ELSE
        INSERT INTO Notes (ScenarioName1, ScenarioNote1, ScenarioName2, ScenarioNote2, ScenarioName3, ScenarioNote3, UserId)
        VALUES (@scenarioname1, @scenarionotes1, @scenarioname2, @scenarionotes2, @scenarioname3, @scenarionotes3, @userId)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    HelperMethods.BindValueString(command, "@scenarioname1", notes.ScenarioName1);
                    HelperMethods.BindValueString(command, "@scenarionotes1", notes.ScenarioNotes1);
                    HelperMethods.BindValueString(command, "@scenarioname2", notes.ScenarioName2);
                    HelperMethods.BindValueString(command, "@scenarionotes2", notes.ScenarioNotes2);
                    HelperMethods.BindValueString(command, "@scenarioname3", notes.ScenarioName3);
                    HelperMethods.BindValueString(command, "@scenarionotes3", notes.ScenarioNotes3);
                    HelperMethods.BindValueInt(command, "@userId", notes.UserId);
                    command.ExecuteNonQuery();
                }
            }
            return notes;
        }
        catch (SqlException e)
        {
            throw new Exception(e.Message);
        }
    }
}

