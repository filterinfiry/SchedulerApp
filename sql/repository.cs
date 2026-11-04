using Radzen.Blazor.Rendering;
using Dapper;
using MySql.Data.MySqlClient;
using SchedulerApp.Model;
using System.Drawing;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

namespace SchedulerApp.sql
{

    public class repository
    {
      
        readonly string _connectionString;
        public repository(string connectionstring) 
        {
            _connectionString = connectionstring;
        }


        public async Task<bool> CreateUser(string name, int rol, string color)
        {
            try
            {
                string sql = "INSERT INTO usuariostareas (Nombre,rol,color) Value (@name,@rol,@color)";

                using (var db = new MySqlConnection(_connectionString))
                {
                    return await db.ExecuteAsync(sql, new { name, rol,color }) > 0;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                string sql = "Delete from usuariostareas where idusuarios = @id";

                using (var db = new MySqlConnection(_connectionString))
                {
                    return await db.ExecuteAsync(sql, new { id }) > 0;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        public async Task<List<usuariostareas>> GetUsuariostareasAsync()
        {
            string sql = "Select IdUsuarios,U.Nombre, R.nombre as rolname,R.idrol,U.color " +
                " FROM usuariostareas U " +
                "INNER JOIN sys_rol R ON R.idrol = U.rol";

            using (var db = new MySqlConnection(_connectionString))
            {
                var list = await db.QueryAsync<usuariostareas>(sql); 
                return list.ToList();
            }
        }

        public async Task<List<usuariostareas>> GetUsuariostareasByRolAsync(int rol)
        {
            string sql = "Select IdUsuarios,U.Nombre, R.nombre as rolname,R.idrol,U.color " +
                " FROM usuariostareas U " +
                "INNER JOIN sys_rol R ON R.idrol = U.rol " +
                "where u.rol = @rol";

            using (var db = new MySqlConnection(_connectionString))
            {
                var list = await db.QueryAsync<usuariostareas>(sql,new {rol});
                return list.ToList();
            }
        }

        public async Task<List<RangoTiempos>> GetRangoTiemposAsync(int idUser, DateTime startDay)
        {
            try
            {
                string sql = "Select Fecha as StartTask, FechaFin as EndTask,idUsuario FROM Citas " +
                       "Where Fecha < @end  and FechaFin > @start and IdUsuario= @idUsuario ";

                using (var db = new MySqlConnection(_connectionString))
                {
                    var list = await db.QueryAsync<RangoTiempos>(sql, new { idUsuario = idUser, start = startDay, end = startDay.AddDays(1) });
                    return list.ToList();
                }
            }
            catch (Exception)
            {

                return new List<RangoTiempos>();
            }
        }


        public async Task<List<sys_rol>> GetRolType()
        {
            string sql = "Select idrol, nombre from sys_rol";
            using (var db = new MySqlConnection(_connectionString))
            {
                 var list = await db.QueryAsync<sys_rol>(sql);
                return list.ToList();
            }
        }

        public async Task<List<PrevisionCitas>> GetPrevisionCitasByMoth(DateTime date)
        {
            string sql = "Select id, idUsuario, NombreCliente,CorreoCliente, Fecha,Tiempo,Descripcion,Matricula,u.color,FechaFin, u.Nombre as Usuario " +
                " From Citas c " +
                "left join usuariostareas u on u.idUsuarios = c.idUsuario " +
                " where Fecha between @start and @end";

            try
            {
                using (var db = new MySqlConnection(_connectionString))
                {
                    var list = await db.QueryAsync<PrevisionCitas>(sql, new
                    {
                        start = date.Date,
                        end = date.Date.AddMonths(18)
                    });

                    return list.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<List<PrevisionCitas>> GetPrevisionCitasByMothWhitRol(DateTime date, int rol)
        {
            string sql = "Select id, idUsuario, NombreCliente,CorreoCliente, Fecha,Tiempo,Descripcion,Matricula,u.color,FechaFin, u.Nombre as Usuario " +
                " From Citas c " +
                "left join usuariostareas u on u.idUsuarios = c.idUsuario " +
                " where Fecha between @start and @end ";
            if (rol > 0)
            {
                sql += "and u.rol=@rol";
            }

            try
            {
                using (var db = new MySqlConnection(_connectionString))
                {
                    var list = await db.QueryAsync<PrevisionCitas>(sql, new
                    {
                        start = date.Date,
                        end = date.Date.AddMonths(18),
                        rol = rol
                    });

                    return list.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<PrevisionCitas> GetPrevisionCitasByMoth(DateTime start,DateTime end)
        {
            string sql = "Select id, idUsuario, NombreCliente,CorreoCliente, Fecha,Tiempo,Descripcion,Matricula,u.color,FechaFin, u.Nombre as Usuario " +
                " From Citas c " +
                "left join usuariostareas u on u.idUsuarios = c.idUsuario " +
                " where Fecha between @start and @end";

            try
            {
                using (var db = new MySqlConnection(_connectionString))
                {
                    var list = db.Query<PrevisionCitas>(sql, new
                    {
                        start,
                        end 
                    });

                    return list.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<PrevisionCitas>> GetPrevisionCitasByMothAndUser(DateTime date, int idUsuario)
        {
            string sql = "Select id, idUsuario, NombreCliente,CorreoCliente, Fecha,Tiempo,Descripcion,Matricula,u.color,FechaFin, u.Nombre as Usuario " +
                " From Citas c " +
                "left join usuariostareas u on u.idUsuarios = c.idUsuario " +
                " where Fecha between @start and @end and c.idUsuario = @idUsuario";

            try
            {
                using (var db = new MySqlConnection(_connectionString))
                {
                    var list = await db.QueryAsync<PrevisionCitas>(sql, new
                    {
                        start = date.Date,
                        end = date.Date.AddMonths(18),
                        idUsuario = idUsuario
                    });

                    return list.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<PrevisionCitas>> GetPrevisionCitasByMothAndUserRol(DateTime date, int idUsuario,int rol)
        {
            string sql = "Select id, idUsuario, NombreCliente,CorreoCliente, Fecha,Tiempo,Descripcion,Matricula,u.color,FechaFin, u.Nombre as Usuario " +
                " From Citas c " +
                "left join usuariostareas u on u.idUsuarios = c.idUsuario " +
                " where Fecha between @start and @end ";
            if(idUsuario != 0)
            {
                sql += "and c.idUsuario = @idUsuario";
            }
            if(rol != 0)
            {
                sql += " and u.rol=@rol ";
            }
                 

            try
            {
                using (var db = new MySqlConnection(_connectionString))
                {
                    var list = await db.QueryAsync<PrevisionCitas>(sql, new
                    {
                        start = date.Date,
                        end = date.Date.AddMonths(18),
                        idUsuario = idUsuario,
                        rol= rol
                    });

                    return list.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        public async Task<PrevisionCitas> GetPrevisionCitasAsyncById(int id)
        {
            string sql = "Select id, idUsuario, NombreCliente,CorreoCliente, Fecha,Tiempo,Descripcion,Matricula,FechaFin From Citas where id = @id";

            try
            {
                using (var db = new MySqlConnection(_connectionString))
                {

                    return await db.QueryFirstAsync<PrevisionCitas>(sql,new {id});
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteCita(int id)
        {
            try
            {
                string sql = "Delete from Citas where id = @id";

                using (var db = new MySqlConnection(_connectionString))
                {
                    return await db.ExecuteAsync(sql, new { id }) > 0;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        public async Task<bool> InsertCita(PrevisionCitas citas)
        {
            try
            {
                string sql = "Insert into citas (idUsuario,NombreCliente,CorreoCliente,Fecha,Tiempo,Descripcion,Matricula,FechaFin) " +
                        "Values (@idUsuario,@NombreCliente,@CorreoCliente,@Fecha,@Tiempo,@Descripcion,@Matricula,@FechaFin)";

                using (var db = new MySqlConnection(_connectionString))
                {
                    return await db.ExecuteAsync(sql, citas) > 0;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> UpdateCitas(PrevisionCitas citas)
        {
            try
            {
                string sql = "Update citas set NombreCliente=@NombreCliente, CorreoCliente=@CorreoCliente,Fecha=@Fecha,FechaFin=@FechaFin,Tiempo=@Tiempo,Descripcion=@Descripcion,Matricula = @Matricula where id=@id";

                using (var db = new MySqlConnection(_connectionString))
                {
                    return await db.ExecuteAsync(sql, citas) > 0;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> CheckCitas(DateTime start,DateTime end, int idUsuario, int id = 0)
        {
            // id > 0 => estamos editando: excluimos la propia cita para que no se detecte
            // como solapada consigo misma. id = 0 (alta) no excluye nada (no existe id 0).
            string sql = "Select count(*) FROM Citas where Fecha < @end  and FechaFin > @start and IdUsuario= @idUsuario and id <> @id ";

            try
            {
                using (var db = new MySqlConnection(_connectionString))
                {
                    return await db.QueryFirstAsync<int>(sql, new { start, end,idUsuario, id }) == 0;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
