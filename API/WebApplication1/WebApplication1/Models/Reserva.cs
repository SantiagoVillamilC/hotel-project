using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int IdPerfil { get; set; }
        public string Hotel { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int CantidadPersonas { get; set; }
        public string Estado { get; set; } // pendiente, confirmada o Cancelada
    }
}
