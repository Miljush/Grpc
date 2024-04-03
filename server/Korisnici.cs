namespace GrpcGreeter
{
    public class Korisnici
    {
        public Dictionary<int, Korisnik> Baza { get; set; }
        private static Korisnici instanca;
        private static object lockObj = new object();

        private Korisnici()
        {
            Baza = new Dictionary<int, Korisnik>();
        }

        public static Korisnici Instanca()
        {
            if (instanca == null)
            {
                lock (lockObj)
                {
                    if (instanca == null)
                    {
                        instanca = new Korisnici();
                    }
                }
            }

            return instanca;
        }

    }
}
