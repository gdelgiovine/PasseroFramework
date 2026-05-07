using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Passero.Framework.JwtTester
{
    public partial class frmTester : Form
    {
        private string? _jwtToken;

        public frmTester()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Da file .pfx (include chiave privata)
            //var cert = new X509Certificate2("myCert.pfx", "password");
            //var jwtManager = new JwtManager(cert, issuer: "myApp", audience: "myApi");


            //string secretKey = Jwt.JwtManager.GenerateSecretKey();        // default 64 byte
            string secretKey32 = Jwt.JwtManager.GenerateSecretKey(32);    // minimo 32 byte

            var jwtManager = new Jwt.JwtManager(
                "0lQF1JSooP9m5bOFbycHmd6hyt7hP83xz7le0seAZEM=",
                issuer: "myApp",
                audience: "myApi");


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "mario.rossi"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            _jwtToken = jwtManager.CreateToken(claims);

            // Validazione (basta la chiave pubblica — puoi anche usare un .cer)
            bool valid = jwtManager.TryDecodeToken(_jwtToken, out var principal);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_jwtToken))
            {
                MessageBox.Show("Generare prima il JWT.", "PING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var responseBody = await PasseroDemoPingClient.PingAsync("http://localhost:5000", _jwtToken);
            MessageBox.Show(responseBody, "PING Response", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
