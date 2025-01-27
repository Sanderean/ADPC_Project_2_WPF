using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPC_Project_2_WPF.Models.Constraints
{
    public class MongoDBSettings
    {
        public const string ConnectionString = "mongodb+srv://Sanderean:4and002123@geneexpressioncluster.ote90.mongodb.net/?retryWrites=true&w=majority&appName=GeneExpressionCluster";
        public const string DatabaseName = "GeneExpressionDB";
        public const string CollectionName_GeneExpressions = "GeneExpressions";
    }
}
