using System;
using System.Collections.Generic;

namespace work.Models
{
    public class SearchResultEnvelope
    {
        public Result[] results { get; set; }

        public SearchResultEnvelope() : this(false)
        {
        }

        public SearchResultEnvelope(bool dummydata)
        {
            throw new NotImplementedException();
        }

    }

    public class Result
    {
        public string upc;
        public string name;
        public string description;
        public string image;
        public string manufacturer;
        public string model;
        public string sku;
        public string type;
        public string url;
    }

    public static class ResultExtension
    {
        public static Result ToResult(this Google.Cloud.BigQuery.V2.BigQueryRow row)
        {
            //description,image,manufacturer,model,name,price,shipping,sku,type,upc,url

            var ret = new Result()
            {
                upc = row["upc"].ToString(),
                name = row["name"].ToString(),
                description = row["description"].ToString(),
                image = row["image"].ToString(),
                manufacturer = (row["manufacturer"] ?? string.Empty).ToString(),
                model = (row["model"] ?? string.Empty).ToString(),
                sku = row["sku"].ToString(),
                type = row["type"].ToString(),
                url = row["url"].ToString(),
            };

            return ret;
        }

        public static Result FromSearchHit(this Newtonsoft.Json.Linq.JToken token)
        {
            var ret = new Result()
            {
                upc = token["upc"].ToString(),
                name = token["name"].ToString(),
                description = token["description"].ToString(),
                image = token["image"].ToString(),
                manufacturer = (token["manufacturer"] ?? string.Empty).ToString(),
                model = (token["model"] ?? string.Empty).ToString(),
                sku = token["sku"].ToString(),
                type = token["type"].ToString(),
                url = token["url"].ToString()
            };

            return ret;
        }

    }
}