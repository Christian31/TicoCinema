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
}
