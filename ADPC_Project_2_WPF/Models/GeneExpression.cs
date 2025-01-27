using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPC_Project_2_WPF.Models
{
    public class GeneExpression
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("PatientId")]
        public string PatientId { get; set; }

        [BsonElement("CancerCohort")]
        public string CancerCohort { get; set; }

        [BsonElement("GeneValues")]
        public Dictionary<string, double> GeneValues { get; set; }
    }
}
