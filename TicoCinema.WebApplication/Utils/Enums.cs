using System.ComponentModel;

namespace TicoCinema.WebApplication.Utils.Enums
{
    public enum Gender: int
    {
        [Description("Femenino")]
        Femenino = 1,

        [Description("Masculino")]
        Masculino = 2,

        [Description("Otro")]
        Otro = 3
    }

    public enum CinemaDesign : int
    {
        [Description("Un bloque")]
        UnBloque = 1,

        [Description("Dos bloques")]
        DosBloques = 2,

        [Description("Tres bloques")]
        TresBloques = 3
    }

    public enum HoursRange : int
    {
        [Description("Ninguno")]
        None = 0,

        [Description("3 horas")]
        Three = 3,

        [Description("4 horas")]
        Four = 4,

        [Description("5 horas")]
        Five = 5,

        [Description("6 horas")]
        Six = 6
    }
}
