using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcGreeter;
namespace GrpcGreeter.Services
{
    public class ElfakService : Elfak.ElfakBase
    {
        public override Task<Poruka> DodajKorisnika(Korisnik request, ServerCallContext context)
        {
            if (Korisnici.Instanca().Baza.ContainsKey(request.BrojIndeksa))
            {
                return Task.FromResult(new Poruka { Text = "Korisnik vec postoji" });
            }
            Korisnici.Instanca().Baza.Add(request.BrojIndeksa, request);
            return Task.FromResult(new Poruka { Text = $"Korisnik sa indeksom {request.BrojIndeksa}" });
        }
        public override Task<Poruka> ObrisiKorisnika(Indeks request, ServerCallContext context)
        {
            if (Korisnici.Instanca().Baza.ContainsKey(request.Indeks_))
            {
                Korisnici.Instanca().Baza.Remove(request.Indeks_);
                return Task.FromResult(new Poruka { Text = "Uspesno je izbrisan korisnik sa indeksom"+request.Indeks_ });
            }
            else
            {
                return Task.FromResult(new Poruka { Text = "Ne postoji korisnik sa tim indeksom" });
            }
        }
        public override async Task VratiKorisnike(IndeksOdDo request, IServerStreamWriter<Korisnik> responseStream, ServerCallContext context)
        {
            var korsinici=Korisnici.Instanca().Baza.OrderBy(s => s.Key).Where(s => s.Key >= request.OdBrojaIndeksa && s.Key <= request.DoBrojaIndeksa);
            foreach (var k in korsinici)
            {
                await responseStream.WriteAsync(new Korisnik
                {
                    BrojIndeksa = k.Key,
                    Ime = k.Value.Ime,
                    Prezime = k.Value.Prezime,
                    Adresa = new Adresa
                    {
                        Ulica = k.Value.Adresa.Ulica,
                        Broj = k.Value.Adresa.Broj,
                        Grad = k.Value.Adresa.Grad
                    },
                    Telefoni = { k.Value.Telefoni }

                });
            }
        }
    }
}