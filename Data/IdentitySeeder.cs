
using Microsoft.AspNetCore.Identity;

namespace DW01.Data
{
    public static class IdentitySeeder
    {
        public static async Task InicializarAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Administrador", "Usuario" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            await CriarUsuarioAsync(
                userManager, "admin", "Admin@123", "Administrador");

            await CriarUsuarioAsync(
                userManager, "usuario", "User@123", "Usuario");
        }

        private static async Task CriarUsuarioAsync(
            UserManager<IdentityUser> userManager,
            string nome,
            string senha,
            string role)
        {
            var usuario = await userManager.FindByNameAsync(nome);

            if (usuario == null)
            {
                usuario = new IdentityUser
                {
                    UserName = nome,
                    Email = $"{nome}@dw01.local",
                    EmailConfirmed = true
                };

                var resultado = await userManager.CreateAsync(
                    usuario, senha);

                if (!resultado.Succeeded)
                {
                    var erros = string.Join("; ",resultado.Errors.Select(e => e.Description));

                    throw new InvalidOperationException($"Erro ao criar o usuario {nome}: {erros}");
                }
            }

            if (!await userManager.IsInRoleAsync(usuario, role))
            {
                var resultadoRole = await userManager.AddToRoleAsync(usuario, role);

                if (!resultadoRole.Succeeded)
                {
                    var erros = string.Join("; ",resultadoRole.Errors.Select(e => e.Description));

                    throw new InvalidOperationException($"Erro ao atribuir a role {role}: {erros}");
                }
            }
        }
    }
}
