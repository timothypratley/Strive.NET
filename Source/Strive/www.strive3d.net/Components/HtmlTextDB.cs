using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using www.strive3d.net;

namespace www.strive3d.net {

    //*********************************************************************
    //
    // HtmlTextDB Class
    //
    // Class that encapsulates all data logic necessary to add/query/delete
    // HTML/text within the Portal database.
    //
    //*********************************************************************

    public class HtmlTextDB {


        //*********************************************************************
        //
        // GetHtmlText Method
        //
        // The GetHtmlText method returns a SqlDataReader containing details
        // about a specific item from the HtmlText database table.
        //
        // Other relevant sources:
        //     + <a href="GetHtmlText.htm" style="color:green">GetHtmlText Stored Procedure</a>
        //
        //*********************************************************************

        public SqlDataReader GetHtmlText(int moduleId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_GetHtmlText", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleId;
            myCommand.Parameters.Add(parameterModuleID);

			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
			
			// Return the datareader 
			return result;
		}


        //*********************************************************************
        //
        // UpdateHtmlText Method
        //
        // The UpdateHtmlText method updates a specified item within
        // the HtmlText database table.
        //
        // Other relevant sources:
        //     + <a href="UpdateHtmlText.htm" style="color:green">UpdateHtmlText Stored Procedure</a>
        //
        //*********************************************************************

        public void UpdateHtmlText(int moduleId, String desktopHtml, String mobileSummary, String mobileDetails) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_UpdateHtmlText", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleId;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterDesktopHtml = new SqlParameter("@DesktopHtml", SqlDbType.NText);
            parameterDesktopHtml.Value = desktopHtml;
            myCommand.Parameters.Add(parameterDesktopHtml);

            SqlParameter parameterMobileSummary = new SqlParameter("@MobileSummary", SqlDbType.NText);
            parameterMobileSummary.Value = mobileSummary;
            myCommand.Parameters.Add(parameterMobileSummary);

            SqlParameter parameterMobileDetails = new SqlParameter("@MobileDetails", SqlDbType.NText);
            parameterMobileDetails.Value = mobileDetails;
            myCommand.Parameters.Add(parameterMobileDetails);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}

