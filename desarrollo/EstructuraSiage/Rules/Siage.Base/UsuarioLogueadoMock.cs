namespace Siage.Base
{
    public class UsuarioLogueadoMock
    {
        #region Atributos
        
        private static UsuarioLogueadoMock _instancia;
        private static int id = 1;
        private static int escuelaId = 2;
        private static string nombre = "Elvis Chacha";
        private static string tipoUsuarioLogueado = "PERSONA";
        private static string login = "administrador";

        #endregion

        #region Propiedades

        public static UsuarioLogueadoMock Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new UsuarioLogueadoMock();
                }
                return _instancia;
            }
        }

        public int Id
        {
            get { return id; }
        }

        public int EscuelaId
        {
            get { return escuelaId; }
        }

        public string Nombre
        {
            get { return nombre; }
        }

        public string TipoUsuarioLogueado
        {
            get { return tipoUsuarioLogueado; }
        }

        public string Login { get { return login; } }

        #endregion

        #region Soporte

        private UsuarioLogueadoMock() { }

        #endregion               
    }
}
