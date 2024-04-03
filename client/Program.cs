using System.Threading.Tasks;
using GrpcGreeterClient;
using Grpc.Net.Client;
using Grpc.Core;


// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:7108");
var client = new Elfak.ElfakClient(channel);
string unos;

do
{
    Console.WriteLine("\n\tDodaj korisnika - a1");
    Console.WriteLine("\tDodaj vise studenata - am");
    Console.WriteLine("\tObrisi studenta - d1");
    Console.WriteLine("\tObrisi vise studenata - dm");
    Console.WriteLine("\tPreuzmi studenta - g1");
    Console.WriteLine("\tPreuzmi vise studenata - gm");
    Console.WriteLine("\tPrekini izvrsenje programa - x");
    unos = Console.ReadLine();
    switch (unos)
    {
        case "a1":
            await DodajKorisnika();
            break;
        case "d1":
            await ObrisiKorisnika();
            break;
        case "gm":
            await VratiKorisnike();
            break;
        default: break;
    }
} while (unos != "x");

async Task VratiKorisnike()
{
    Console.WriteLine("Unesite donju i gornju granicu broja indeksa.");
    int odBrojaIndeksa = Int32.Parse(Console.ReadLine());
    int doBrojIndeksa = Int32.Parse(Console.ReadLine());
    try
    {
        var call = client.VratiKorisnike(new IndeksOdDo
        {
            OdBrojaIndeksa = odBrojaIndeksa,
            DoBrojaIndeksa = doBrojIndeksa
        });

        await foreach (var resp in call.ResponseStream.ReadAllAsync())
        {
            Console.WriteLine($"{resp.BrojIndeksa}");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}
async Task ObrisiKorisnika()
{
    Console.WriteLine("Unesite broj indeksa za brisanje.");
    int brojIndeksa = Int32.Parse(Console.ReadLine());

    try
    {
        var resp = await client.ObrisiKorisnikaAsync(new Indeks
        {
           Indeks_=brojIndeksa

        });
        Console.WriteLine(resp.Text);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}
async Task DodajKorisnika()
{
    Console.WriteLine("Unesite broj indeksa, a zatim ime,prezime studenta, adresa,brojtelefona, telefoni.");
    int brojIndeksa = Int32.Parse(Console.ReadLine());
    string ime = Console.ReadLine();
    string prezime = Console.ReadLine();
    string ulica = Console.ReadLine();
    int broj = Int32.Parse(Console.ReadLine());
    string grad = Console.ReadLine();
    int brojTelefona = Int32.Parse(Console.ReadLine());
    List<BrojTelefona> telefoni = new List<BrojTelefona>();
    for (int i = 0; i < brojTelefona; i++)
    {
        Console.WriteLine($"Unesite broj telefona {i + 1}:");
        string brojTelefonaStr = Console.ReadLine();
        telefoni.Add(new BrojTelefona { Broj = brojTelefonaStr });
    }

    try
    {
        var resp = await client.DodajKorisnikaAsync(new Korisnik
        {
            BrojIndeksa = brojIndeksa,
            Ime = ime,
            Prezime = prezime,
            Adresa = new Adresa
            {
                Ulica = ulica,
                Broj = broj,
                Grad = grad
            },
            Telefoni = { telefoni }

        });
        Console.WriteLine(resp.Text);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}
