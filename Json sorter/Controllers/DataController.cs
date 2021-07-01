using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Json_sorter.classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

        //http://<address>/getcontainer/Search/<SearchTerm>/<ContainerCategory>
        //http://<address>/getcontainer/Search/<SearchTerm>/<ContainerCategory>/?filters=<filter1>;<filter2>


namespace Json_sorter.Controllers
{
    [ApiController]
    [Route("getcontainer")]
    public class DataController : ControllerBase
    {
        GetJsonData getJsonData = new GetJsonData();
        private readonly ILogger<DataController> _logger;

        //GET: original data (filter if filterstring present)
        [HttpGet]
        public string Get(string sortBy, [FromQuery] string filters)
        {
            
            //JArray jArray = getJsonData.GetJsonFile(filters);  //from file
            
            
            //get data from web/localhost
            JArray jArray = getJsonData.GetJsonFromWeb(filters); 

            dynamic result = JsonConvert.SerializeObject(jArray, Formatting.Indented);
            return result;
        }

        //GET: sort and return sorted data (filter if filterstring present)
        [HttpGet("Sort/{sortBy}")]
        public string SortGet(string sortBy, string descending, [FromQuery]string filters)
        {

            //JArray jArray = getJsonData.GetJsonFile(filters);  //from file
            JArray jArray = getJsonData.GetJsonFromWeb(filters); //from web/localhost

            //ascending search
            if (sortBy != null)
            {
                getJsonData.SortData(jArray, sortBy, filters,false);
                dynamic result = JsonConvert.SerializeObject(getJsonData.sortedData, Formatting.Indented);
                return result;
            }
            //error test
            else
            {
                dynamic result = JsonConvert.SerializeObject("No valid sorting parameters", Formatting.Indented);
                return result;
            }
        }

        //GET: sort and return sorted data (filter if filterstring present)
        [HttpGet("Sort/Desc/{sortBy}")]
        public string SortGet(string sortBy, [FromQuery] string filters)
        {

            //JArray jArray = getJsonData.GetJsonFile(filters);  //from file
            JArray jArray = getJsonData.GetJsonFromWeb(filters); //from web/localhost

            //descending sort
            if (sortBy != null)
            {
                getJsonData.SortData(jArray,sortBy, filters,true);
                dynamic result = JsonConvert.SerializeObject(getJsonData.sortedData, Formatting.Indented);
                return result;
            }

            //error test
            else
            {
                dynamic result = JsonConvert.SerializeObject("No valid sorting parameters", Formatting.Indented);
                return result;
            }
        }

        //GET: search and return search result data (filter if filterstring present)
        [HttpGet("Search/{searchTerm}/{searchTopic}")]
        public string SearchGet(string searchTerm, string searchTopic, [FromQuery] string filters)
        {
            //JArray jArray = getJsonData.GetJsonFile(filters);  //from file
            JArray jArray = getJsonData.GetJsonFromWeb(filters); //from web/localhost

            //search
            if (searchTerm !=null && searchTopic != null) { 
                getJsonData.SearchData(jArray, searchTerm, searchTopic, filters);                
                dynamic result = JsonConvert.SerializeObject(getJsonData.searchedData, Formatting.Indented);
                return result;
            }

            //error test
            else
            {
                dynamic result = JsonConvert.SerializeObject("No valid searchterm:\t", Formatting.Indented);
                return result;                
            }
        }
    }
}
