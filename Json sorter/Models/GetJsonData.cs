using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Json_sorter.classes
{
    public class GetJsonData
    {
        StreamReader streamReader;

        public JArray originalData;
        public JArray sortedData;
        public JArray searchedData;
        public JArray filteredData;


        /// <summary>
        /// Reads jasondata (array) from file and creates a JArray
        /// </summary>
        /// <returns>JArray - jsondata</returns>
        public Newtonsoft.Json.Linq.JArray GetJsonFile(string filters)
        {
            string[] filterArray;
            if (filters != null && filters.Length >= 1)
            {
                filterArray = filters.ToLower().Split(';');
            }
            else filterArray = new string[] { };
            using (streamReader = new StreamReader("Container_assignment.json")) //"test_data.json"))
            {
                var jsonData = streamReader.ReadToEnd();
                originalData = JArray.Parse(jsonData);
            }
            if (filterArray.Length >= 1)
            {
                originalData = FilterData(originalData, filterArray);
            }
            return originalData;
        }

        /// <summary>
        /// Reads jasondata (array) from website/localhost and creates a JArray
        /// </summary>
        /// <returns>JArray - jsondata</returns>
        public JArray GetJsonFromWeb(string filters)
        {
            string[] filterArray;
            if (filters != null && filters.Length >= 1)
            {
                filterArray = filters.ToLower().Split(';');
            }
            else filterArray = new string[] { };
            string json = (new WebClient()).DownloadString("Container_assignment.json"); // "test_data.json"); 
            originalData = JArray.Parse(json);
            if (filterArray.Length >= 1)
            {
                originalData = FilterData(originalData, filterArray);
            }
            return originalData;
        }

        /// <summary>
        /// Sort data according to input and returned sorted data.
        /// </summary>
        /// <param name="sortBy">Value to sort by - Ascending sorting </param>
        /// <param name="sortDesc">Value to sort by - Descending sorting </param>
        /// <returns></returns>
        public JArray SortData(JArray jsonArray, string sortBy, string filters, bool descending)
        {
            string[] filterArray;
            if (filters != null && filters.Length >= 1)
            {
                filterArray = filters.ToLower().Split(';');
            }
            else filterArray = new string[] { };

            sortedData = jsonArray;
            var orderedResult = new JArray(sortedData.OrderBy(X => X[sortBy]));

            if (descending)
            {
                //orderedResult = new JArray(jsonArray.OrderBy(X => X[sortBy]).ThenByDescending(x => x[sortBy]));
                orderedResult = new JArray(jsonArray.OrderByDescending(X => X[sortBy]));
            }
            sortedData = orderedResult;  
            //filter data if filter parameters
            if (filterArray.Length >= 1)
            {
                sortedData = FilterData(sortedData, filterArray);
            }
            return sortedData;
        }

        /// <summary>
        /// Search data and return items machting serachterm in the searchtopic
        /// </summary>
        /// <param name="jsonArray">Data set (JArray) with container information</param>
        /// <param name="searchTerm">TextString to search for in searchtopic</param>
        /// <param name="searchTopic">Topic (containerparam) to search in </param>
        /// <returns></returns>
        public JArray SearchData(JArray jsonArray, string searchTerm, string searchTopic, string filters)
        {
            string[] filterArray;
            if (filters != null && filters.Length >= 1)
            {
                filterArray = filters.ToLower().Split(';');
            }
            else filterArray = new string[] { };
            searchedData = new JArray();   
            for (int i = 0; i <= jsonArray.Count -1; i++)
            {                                
                JObject tmpContainer = (JObject)jsonArray[i];
                foreach(KeyValuePair<string, JToken> containerParams in tmpContainer)
                {
                    if ((searchTopic.ToLower() == (containerParams.Key.ToLower())) && containerParams.Value.ToString().ToLower().Contains(searchTerm.ToLower()))//.Contains(,StringComparison.Ordinal))
                    {
                        searchedData.Add(tmpContainer);
                    }
                }               
            }
            //filter data if filter parameters
            if (filterArray.Length >= 1)
            {
                searchedData = FilterData(searchedData, filterArray);
            }
            return searchedData;
        }

        /// <summary>
        /// Filters containerdata according to selected filter strings
        /// </summary>
        /// <param name="jsonArray">Array (JArray) of  containers</param>
        /// <param name="filterArray">Filter strings parameters</param>
        /// <returns></returns>
        public JArray FilterData(JArray jsonArray, string[] filterArray)
        {
            bool filterMatch = false;
            filteredData = new JArray();
            for (int i = 0; i <= jsonArray.Count - 1; i++)
            {
                JObject tmpContainer = (JObject)jsonArray[i];
                foreach (KeyValuePair<string, JToken> containerParams in tmpContainer)
                {                    
                    if (filterArray.Contains(containerParams.Value.ToString().ToLower())) //.ToLower())) && containerParams.Value.ToString().ToLower().Contains(searchTerm.ToLower()))//.Contains(,StringComparison.Ordinal))
                    {
                        filterMatch= true;
                    }
                }
                if (!filterMatch) filteredData.Add(tmpContainer);
                filterMatch= false;
            }
            //if (filteredData.Count < 1)
            //{
            //    filteredData.Clear();
            //    //searchedData[i].Remove();
            //}
            return filteredData;
        }
    }
}
