using ApiNotificaciones.Model;
using ApiNotificaciones.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Sprache;

namespace ApiNotificaciones.Services
{
    public class BancoRepository : IBancoRepository
    {
        public BancoRepository(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        private SqlConnection sqlConnection;

        public bool InsertarNotificacion(Notificacion notificacion, int estado)
        {
            //var result = 1;
            var sql = @"INSERT INTO [dbo].[H_Notificaciones]
                            (id_ref,mensaje,telefono,tipo, fecha,estado) VALUES 
                                (@id_ref,@mensaje,@telefono,@tipo,@fecha, @estado)";

            var result = sqlConnection.Execute(sql.ToString(), new
            {
                id_ref = notificacion.id,
                mensaje = notificacion.mstxt,
                telefono = notificacion.telefono,
                tipo = notificacion.tipo,
                fecha = DateTime.Now,
                estado = estado
            });

            return result > 0;
        }
    }
}