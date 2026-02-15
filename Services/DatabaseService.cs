using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HistoriaClinicaApp.Models;

namespace HistoriaClinicaApp.Services
{
    public class DatabaseService
    {
        private readonly Supabase.Client _client;

        public DatabaseService()
        {
            _client = SupabaseService.GetClient();
        }

        public Task<List<Paciente>> ObtenerPacientes()
        {
            return Task.Run(async () =>
            {
                var response = await _client.From<Paciente>().Get();
                return response.Models;
            });
        }

        public Task CrearPaciente(Paciente paciente)
        {
            return Task.Run(async () => await _client.From<Paciente>().Insert(paciente));
        }

        public Task ActualizarPaciente(Paciente paciente)
        {
            return Task.Run(async () => await _client.From<Paciente>().Update(paciente));
        }

        public Task EliminarPaciente(long id)
        {
            return Task.Run(async () => await _client.From<Paciente>().Where(x => x.Id == id).Delete());
        }

        public Task<HistoriaClinica> ObtenerHistoriaClinica(long pacienteId)
        {
            return Task.Run(async () =>
            {
                var response = await _client.From<HistoriaClinica>()
                    .Where(x => x.PacienteId == pacienteId)
                    .Single();
                return response;
            });
        }

        public Task CrearHistoriaClinica(HistoriaClinica historia)
        {
            return Task.Run(async () => await _client.From<HistoriaClinica>().Insert(historia));
        }

        public Task ActualizarHistoriaClinica(HistoriaClinica historia)
        {
            return Task.Run(async () => await _client.From<HistoriaClinica>().Update(historia));
        }

        public Task<List<Consulta>> ObtenerConsultas(long pacienteId)
        {
            return Task.Run(async () =>
            {
                var response = await _client.From<Consulta>()
                    .Where(x => x.PacienteId == pacienteId)
                    .Get();
                var consultas = response.Models;
                foreach (var c in consultas)
                {
                    var usuario = await _client.From<Usuario>().Where(x => x.Id == c.UsuarioId).Single();
                    c.NombreUsuario = usuario?.NombreUsuario ?? "Desconocido";
                }
                return consultas;
            });
        }

        public Task CrearConsulta(Consulta consulta)
        {
            return Task.Run(async () => await _client.From<Consulta>().Insert(consulta));
        }

        public Task<List<Estudio>> ObtenerEstudios(long pacienteId)
        {
            return Task.Run(async () =>
            {
                var response = await _client.From<Estudio>()
                    .Where(x => x.PacienteId == pacienteId)
                    .Get();
                var estudios = response.Models;
                foreach (var e in estudios)
                {
                    var usuario = await _client.From<Usuario>().Where(x => x.Id == e.UsuarioId).Single();
                    e.NombreUsuario = usuario?.NombreUsuario ?? "Desconocido";
                }
                return estudios;
            });
        }

        public Task CrearEstudio(Estudio estudio)
        {
            return Task.Run(async () => await _client.From<Estudio>().Insert(estudio));
        }

        public Task ActualizarEstudio(Estudio estudio)
        {
            return Task.Run(async () => await _client.From<Estudio>().Update(estudio));
        }

        public Task<List<ArchivoMedico>> ObtenerArchivosMedicos(long pacienteId)
        {
            return Task.Run(async () =>
            {
                var response = await _client.From<ArchivoMedico>()
                    .Where(x => x.PacienteId == pacienteId)
                    .Get();
                return response.Models;
            });
        }

        public Task CrearArchivoMedico(ArchivoMedico archivo)
        {
            return Task.Run(async () => await _client.From<ArchivoMedico>().Insert(archivo));
        }

        public Task EliminarArchivoMedico(long id)
        {
            return Task.Run(async () => await _client.From<ArchivoMedico>().Where(x => x.Id == id).Delete());
        }

        public Task<List<Usuario>> ObtenerTodosUsuarios()
        {
            return Task.Run(async () =>
            {
                var response = await _client.From<Usuario>().Get();
                var usuarios = response.Models;
                foreach (var u in usuarios)
                {
                    var rol = await _client.From<Rol>().Where(x => x.Id == u.RolId).Single();
                    u.Rol = rol;
                }
                return usuarios;
            });
        }

        public Task CrearUsuario(Usuario usuario)
        {
            return Task.Run(async () => await _client.From<Usuario>().Insert(usuario));
        }

        public Task ActualizarUsuario(Usuario usuario)
        {
            return Task.Run(async () => await _client.From<Usuario>().Update(usuario));
        }

        public Task<List<Rol>> ObtenerRoles()
        {
            return Task.Run(async () =>
            {
                var response = await _client.From<Rol>().Get();
                return response.Models;
            });
        }
    
        public Task ResetearPassword(long usuarioId, string nuevoPassword)
        {
            return Task.Run(async () =>
            {
                var usuario = await _client.From<Usuario>().Where(x => x.Id == usuarioId).Single();
                if (usuario != null)
                {
                    usuario.PasswordHash = AuthService.HashPassword(nuevoPassword);
                    await _client.From<Usuario>().Update(usuario);
                }
            });
        }}
}



