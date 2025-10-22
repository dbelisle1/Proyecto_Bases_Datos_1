using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.Models;
using System.Data;

namespace Proyecto_Inmuebles.DBConnection
{
    public class OracleDBConnection
    {
        private readonly string _connString;
        public OracleDBConnection(string connString = ValoresGlobales.ConnString) => _connString = connString;

        /*
         var rows = await OracleExec.SelectAsync(
    ConnString,
    "SELECT ID, NAME, CREATED_AT FROM STUDENTS WHERE ID >= :minId ORDER BY ID",
    new[] { OracleExec.In("minId", 0) }
);

// use later:
foreach (var r in rows)
{
    Console.WriteLine($"#{r["ID"]}: {r["NAME"]} at {r["CREATED_AT"]}");
}*/
        public async Task<List<Dictionary<string, object?>>> SelectAsync(
       string sql, IEnumerable<OracleParameter>? parameters = null)
        {
            var rows = new List<Dictionary<string, object?>>();
            using var conn = new OracleConnection(_connString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand(sql, conn) { BindByName = true };
            if (parameters != null) foreach (var p in parameters) cmd.Parameters.Add(p);

            using var r = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
            var n = r.FieldCount;
            var names = new string[n];
            for (int i = 0; i < n; i++) names[i] = r.GetName(i);

            while (await r.ReadAsync())
            {
                var row = new Dictionary<string, object?>(n, StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < n; i++)
                    row[names[i]] = await r.IsDBNullAsync(i) ? null : r.GetValue(i);
                rows.Add(row);
            }
            return rows;
        }

        /*
         var updated = await OracleExec.UpdateAsync(
     ConnString,
     "UPDATE STUDENTS SET NAME = :name WHERE ID = :id",
     new[] { OracleExec.In("name", "Robert"), OracleExec.In("id", 1) }
 );*/

        /*
         await OracleExec.UpdateAsync(ConnString, "DELETE FROM STUDENTS WHERE ID = :id",
    new[] { OracleExec.In("id", 5) });*/
        public async Task<int> UpdateAsync(
            string sql, IEnumerable<OracleParameter>? parameters = null)
        {
            using var conn = new OracleConnection(_connString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand(sql, conn) { BindByName = true };
            if (parameters != null) foreach (var p in parameters) cmd.Parameters.Add(p);

            return await cmd.ExecuteNonQueryAsync();
        }

        /*
        * var outId = OracleExec.Out("newId", OracleDbType.Int32);
var (affected, outs) = await OracleExec.InsertAsync(
    ConnString,
    "INSERT INTO STUDENTS(NAME) VALUES(:name) RETURNING ID INTO :newId",
    new[] { OracleExec.In("name", "Bob"), outId }
);

var newId = outs["newId"]; // -> the generated ID
        */
        public async Task<(int rows, Dictionary<string, object?> outputs)> InsertAsync(
             string sql, IEnumerable<OracleParameter>? parameters = null)
        {
            using var conn = new OracleConnection(_connString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand(sql, conn) { BindByName = true };
            if (parameters != null) foreach (var p in parameters) cmd.Parameters.Add(p);

            var rows = await cmd.ExecuteNonQueryAsync();

            var outs = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            foreach (OracleParameter p in cmd.Parameters)
            {
                if (p.Direction is ParameterDirection.Output or ParameterDirection.InputOutput
                    or ParameterDirection.ReturnValue)
                {
                    outs[p.ParameterName.TrimStart(':')] =
                        p.Value == DBNull.Value ? null : p.Value;
                }
            }
            return (rows, outs);
        }

       
        public static OracleParameter In(string name, object? value) =>
            new OracleParameter(name, value ?? DBNull.Value) { Direction = ParameterDirection.Input };

        public static OracleParameter Out(string name, OracleDbType type, int size = 0)
        {
            var p = new OracleParameter(name, type) { Direction = ParameterDirection.Output };
            if (size > 0) p.Size = size;
            return p;
        }
    }
}
