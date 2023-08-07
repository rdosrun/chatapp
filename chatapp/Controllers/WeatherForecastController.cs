using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;  // for Controller, [Route], [HttpPost], [FromBody], JsonResult and Json
using System.IO;   // for MemoryStream
using System.Net.Http; // for HttpResponseMessage
using System.Net;  // for HttpStatusCode
using System.Net.Http.Headers;  // for MediaTypeHeaderValue
using System.Data;
using MySqlConnector;
namespace chatapp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    //sql connection
    // set these values correctly for your database server
    private DataTable Run_SQL(string command){
        string cs = "server=127.0.0.1;userid=root;password=password;database=CHATAPP";
        int id =-1;
        MySqlDataReader reader = null;
        DataTable table1 = new DataTable("values");
        table1.Columns.Add("type");
        table1.Columns.Add("value");
        using (MySqlConnection connection = new MySqlConnection(cs)){
            try
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(command, connection))
                {
                    using (reader = cmd.ExecuteReader()){
                        while( reader.Read()){
                            table1.Rows.Add("int",reader.GetInt32(0));
                            table1.Rows.Add("string",reader.GetString(1));

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        return table1;
    }
    }
    // create a DB command and set the SQL statement with parameters


    private String getfile(string path){
        String tmp = "";
        String results ="";
        try{
            StreamReader sr = new StreamReader(path);
            tmp = sr.ReadLine();
            while(tmp!=null){
                results = results+tmp+"\n";
                tmp = sr.ReadLine();
            }
            sr.Close();
        }
        catch(Exception e){
            //throw error page
            results = "<html> "+e.Message+" </html>";
        }
        return results;
    }

    [HttpGet("LandingPage")]
    public ContentResult Get()
    {
        return Content(getfile("./html/landingpage.html"),"text/html");
    }

    [HttpGet("LandingPage.css")]
    public async Task<ContentResult> GetCSS()
    {
        return Content(getfile("./html/landingpage.css"),"text/css");
    }

    [HttpGet("main")]
    public async Task<ContentResult> GetMain()
    {
       return Content (getfile("./html/index.html"),"text/html");
    }

    [HttpGet("mainCSS")]
    public async Task<ContentResult> GetMainCSS()
    {
       return Content (getfile("./html/index.css"),"text/css");
    }

    [Route("Login")]
    [HttpGet]
    public async Task<ContentResult> SignIn(int ID,string password)
    {
        string cmd = "Select * from Login";
        string psswd = "";
        DataTable reader = Run_SQL(cmd);
        foreach (DataRow item in reader.Rows)
        {
            psswd = item["value"].ToString();
        }
        if(psswd != password){
            return Content(getfile("./html/landingpage.html"),"text/html");
        }else{
            return Content (getfile("./html/index.html"),"text/html");
        }
    }

    [HttpGet("DBtest")]
    public async Task<ContentResult> GetID(){
        string cmd = "Select * from Users";
        string id ="-1";
        DataTable reader = Run_SQL(cmd);
        foreach (DataRow item in reader.Rows)
        {
            id = item["value"].ToString();
        }
        return Content(id,"text/html");
    }

}
