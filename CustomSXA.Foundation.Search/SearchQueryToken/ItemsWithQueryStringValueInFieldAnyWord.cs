using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Fields;
using Sitecore.XA.Foundation.Search.Attributes;
using Sitecore.XA.Foundation.Search.Pipelines.ResolveSearchQueryTokens;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;

namespace CustomSXA.Foundation.Search.SearchQueryToken
{
    public class ItemsWithQueryStringValueInFieldAnyWord : ItemsWithQueryStringValueInField
    {
        protected override string TokenPart => nameof(ItemsWithQueryStringValueInFieldAnyWord);

        protected override string Operation { set; get; }

        protected override void UpdateFilter(string paramName, SearchStringModel model, ResolveSearchQueryTokensEventArgs args, int index)
        {
            string queryStringValue = HttpContext.Current.Request.QueryString[paramName];
            if (string.IsNullOrEmpty(queryStringValue))
                queryStringValue = GetURLRefererQueryStringParamValue(paramName);
            if (string.IsNullOrEmpty(queryStringValue))
                return;
            //split the inputs into all words and pass each word for filter with OR condition i.e. should operation
            string[] allWords = queryStringValue.Split(' ');        
            for (int i = 0; i < allWords.Length; i++)
            {
                args.Models.Insert(index, this.BuildModel(paramName, allWords[i])); //pass the field value for filter
                args.Models.Remove(model);
            }
            args.Models.Insert(index, this.BuildModel(paramName, queryStringValue)); //pass the query string value i.e. the input words phrase
            args.Models.Remove(model);
        }
    }
}