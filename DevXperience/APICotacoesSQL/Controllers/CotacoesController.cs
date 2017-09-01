using System.Collections.Generic;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;

namespace APICotacoesSQL.Controllers
{
    public class CotacoesController : ApiController
    {
        [Route("api/cotacoes/{id}")]
        public Cotacao Get(string id)
        {
            using (SqlConnection conexao = new SqlConnection(
                ConfigurationManager.ConnectionStrings["BaseCotacoes"].ConnectionString))
            {
                Cotacao resultado = conexao.QueryFirstOrDefault<Cotacao>(
                    "SELECT * FROM dbo.Cotacoes " +
                    "WHERE Sigla = @idMoeda", new { idMoeda = id });

                TelemetryClient client = new TelemetryClient();
                Dictionary<string, string> dados = new Dictionary<string, string>();
                dados["DadosCotacao"] = JsonConvert.SerializeObject(resultado);
                client.TrackEvent("Dapper", dados);

                return resultado;
            }
        }
    }
}