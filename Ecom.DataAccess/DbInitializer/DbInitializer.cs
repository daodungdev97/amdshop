using Ecom.DataAccess.Data;
using Ecom.Models.Models;
using Ecom.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
          UserManager<IdentityUser> userManager,
          RoleManager<IdentityRole> roleManager,
          ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public void Initializer()
        {
            //migrations if they are not applied
            try
            {
                _db.Database.EnsureCreated();
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

                //create roles if they are not created
                if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_CompanyStaff)).GetAwaiter().GetResult();

                    _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "admin@gmail.com",
                        Email = "daodung1608@gmail.com",
                        Name = "Dung Vip pro",
                        PhoneNumber = "215435421",
                        StreetAddress = "32 NEw York",
                        State = "RR",
                        PostalCode = "321321",
                        City = "Chicagg",
                    }, "Dung@123").GetAwaiter().GetResult();

                    ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "daodung1608@gmail.com");
                    _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult(); ;
                }

                if (!_db.Products.Any())
                {
                    _db.Authors.AddRange(
                            new Author {  Name = "AMD" },
                           new Author {  Name = "INTEL" },
                           new Author { Name = "MSI" },
                           new Author {  Name = "ASROCK" },
                           new Author { Name = "ASUS" },
                           new Author { Name = "GIGABYTE" },
                           new Author { Name = "HKC" },
                           new Author { Name = "AOC" },
                           new Author { Name = "E-DRA" },
                           new Author { Name = "DAREU" },
                             new Author { Name = "CORSAIR" }
                      );
                    _db.Categories.AddRange(
                            new Category {  Name = "Mainboard", DisplayOrder = 1 },
                            new Category {  Name = "CPU", DisplayOrder = 2 },
                            new Category { Name = "VGA", DisplayOrder = 3 },
                            new Category {   Name = "Monitor", DisplayOrder = 4 },
                            new Category {   Name = "Keyboard", DisplayOrder = 5 },
                            new Category { Name = "Mouse", DisplayOrder = 6 }
                        );
                    _db.Collections.AddRange(
                            new Collection {  CollectionName = "Spring" },
                            new Collection { CollectionName = "Winter" },
                            new Collection {  CollectionName = "Summer" },
                            new Collection { CollectionName = "Autumn" }
                        );


                    #region Company
                    _db.Companies.AddRange(

                         new Company
                         {
                             Id = Guid.NewGuid(),
                             Name = "Tech Solution",
                             StreetAddress = "123 Tech St",
                             City = "Tech City",
                             PostalCode = "12121",
                             State = "IL",
                             PhoneNumber = "6669990000"
                         },
                         new Company
                         {
                             Id = Guid.NewGuid(),
                             Name = "Vivid Books",
                             StreetAddress = "999 Vid St",
                             City = "Vid City",
                             PostalCode = "66666",
                             State = "IL",
                             PhoneNumber = "7779990000"
                         },
                         new Company
                         {
                             Id = Guid.NewGuid(),
                             Name = "Readers Club",
                             StreetAddress = "999 Main St",
                             City = "Lala land",
                             PostalCode = "99999",
                             State = "NY",
                             PhoneNumber = "1113335555"
                         } );
                    #endregion

                    #region Product
                    _db.Products.AddRange(
                        new Product
                        {
                            Id = Guid.Parse("33005AD9-DDD8-4DBB-B025-669D781F4B5F"),
                            Title = "A520M PRO",
                            AuthorId = 3,
                            Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                            ISBN = "SWDA520MM",
                            ListPrice = 50,
                            Price = 43,
                            Price50 = 35,
                            Price100 = 30,
                            CategoryId = 1,
                            CollectionId = 1,
                            BuyCount = 100


                        },
                          new Product
                          {
                              Id = Guid.Parse("4271654F-FA30-4A61-89BB-561B61D37EB7"),
                              Title = "B550M PRO VDH",
                              AuthorId = 4,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "CAWB550A",
                              ListPrice = 100,
                              Price = 85,
                              Price50 = 75,
                              Price100 = 65,
                              CategoryId = 1,
                              CollectionId = 3,
                              BuyCount = 22


                          },
                          new Product
                          {
                              Id = Guid.Parse("A180A819-51E9-46C9-B4E0-6E406A6FC658"),
                              Title = "X570 TUF",
                              AuthorId = 5,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "RITOX570A",
                              ListPrice = 150,
                              Price = 120,
                              Price50 = 105,
                              Price100 = 90,
                              CategoryId = 1,
                              CollectionId = 3,
                              BuyCount = 22
                          },
                          new Product
                          {
                              Id = Guid.Parse("33ED75DE-1F7F-41AA-BB3D-FE2AB5F9D265"),
                              Title = "X570 ACE",
                              AuthorId = 3,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "XNDX570AM",
                              ListPrice = 300,
                              Price = 275,
                              Price50 = 250,
                              Price100 = 220,
                              CategoryId = 1,
                              CollectionId = 4,
                              BuyCount = 22
                          },


                          new Product
                          {
                              Id = Guid.Parse("88142B2B-D710-46C1-87B0-CA4FB9E8EE6E"),
                              Title = "RYZEN 3 3100",
                              AuthorId = 1,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "SWD3100",
                              ListPrice = 50,
                              Price = 43,
                              Price50 = 35,
                              Price100 = 30,
                              CategoryId = 2,
                              CollectionId = 2,
                              BuyCount = 33

                          },
                          new Product
                          {
                              Id = Guid.Parse("8361D4AF-7BE2-4F20-B544-D54C1D149F90"),
                              Title = "RYZEN 5 3600",
                              AuthorId = 1,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "CAW3600",
                              ListPrice = 100,
                              Price = 85,
                              Price50 = 75,
                              Price100 = 65,
                              CategoryId = 2,
                              CollectionId = 1,
                              BuyCount = 34


                          },
                          new Product
                          {
                              Id = Guid.Parse("EB5F667A-201A-4727-A367-EDC3F914B32D"),
                              Title = "RYZEN 5 5600X",
                              AuthorId = 1,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "RITOX5600X",
                              ListPrice = 150,
                              Price = 120,
                              Price50 = 105,
                              Price100 = 90,
                              CategoryId = 2,
                              CollectionId = 3,
                              BuyCount = 36
                          },
                          new Product
                          {
                              Id = Guid.Parse("B0AC6BBC-3C5A-4787-89AC-34B43B1EAA61"),
                              Title = "RYZEN 9 5900X",
                              AuthorId = 1,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "XNDX5900XX",
                              ListPrice = 300,
                              Price = 275,
                              Price50 = 250,
                              Price100 = 220,
                              CategoryId = 2,
                              CollectionId = 4,
                              BuyCount = 47
                          },



                            new Product
                            {
                                Id = Guid.Parse("380825B1-2650-4B48-8CC2-D9C35A509BA8"),
                                Title = "RX 550",
                                AuthorId = 1,
                                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                                ISBN = "SWD550OO",
                                ListPrice = 50,
                                Price = 43,
                                Price50 = 35,
                                Price100 = 30,
                                CategoryId = 3,
                                CollectionId = 1,
                                BuyCount = 38

                            },
                          new Product
                          {
                              Id = Guid.Parse("1DB6C932-E9EE-4FFD-BBF0-92EF23061982"),
                              Title = "RX 570",
                              AuthorId = 1,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "CAW570OPP",
                              ListPrice = 100,
                              Price = 85,
                              Price50 = 75,
                              Price100 = 65,
                              CategoryId = 3,
                              CollectionId = 2,
                              BuyCount = 33


                          },
                          new Product
                          {
                              Id = Guid.Parse("1FC51323-7607-4A7F-AAF7-05F09B0B3DFB"),
                              Title = "RX 6600XT",
                              AuthorId = 1,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "RITOX5600X",
                              ListPrice = 150,
                              Price = 120,
                              Price50 = 105,
                              Price100 = 90,
                              CategoryId = 3,
                              CollectionId = 3,
                              BuyCount = 77
                          },
                          new Product
                          {
                              Id = Guid.Parse("EA4D6B02-3EEE-430A-A160-E4D5F5C7C943"),
                              Title = "RX 6800XT",
                              AuthorId = 1,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "XNDX5900XX",
                              ListPrice = 300,
                              Price = 275,
                              Price50 = 250,
                              Price100 = 220,
                              CategoryId = 3,
                              CollectionId = 4,
                              BuyCount = 11
                          },


                            new Product
                            {
                                Id = Guid.Parse("C273F848-A068-42B4-9BAF-382079BB52A1"),
                                Title = "HKC ANT-22F220 (22/FHD/75Hz/1ms)",
                                AuthorId = 7,
                                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                                ISBN = "SWD22F220",
                                ListPrice = 50,
                                Price = 43,
                                Price50 = 35,
                                Price100 = 30,
                                CategoryId = 4,
                                CollectionId = 1,
                                BuyCount = 22

                            },
                          new Product
                          {
                              Id = Guid.Parse("401DA5D4-6F94-44EA-A4C7-3489DB8056E8"),
                              Title = "E-DRA EGM24F75 (23.8/FHD/IPS/75Hz/1ms) ",
                              AuthorId = 9,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "CAW57EGM24F75",
                              ListPrice = 100,
                              Price = 85,
                              Price50 = 75,
                              Price100 = 65,
                              CategoryId = 4,
                              CollectionId = 2,
                              BuyCount = 23


                          },
                          new Product
                          {
                              Id = Guid.Parse("B1317290-360B-4984-B45A-3B8138A11B39"),
                              Title = "Asus VY249HE (23.8/FHD/IPS/75Hz/1ms) ",
                              AuthorId = 5,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "RITOX249H",
                              ListPrice = 150,
                              Price = 120,
                              Price50 = 105,
                              Price100 = 90,
                              CategoryId = 4,
                              CollectionId = 3,
                              BuyCount = 27
                          },
                          new Product
                          {
                              Id = Guid.Parse("8B614A6D-52E9-4093-9948-00CCD45CA6D8"),
                              Title = "AOC CQ32G3SE/74 (31.5/QHD/VA/165HZ/1MS) ",
                              AuthorId = 8,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "XNDCQ32G3SE",
                              ListPrice = 300,
                              Price = 275,
                              Price50 = 250,
                              Price100 = 220,
                              CategoryId = 4,
                              CollectionId = 4,
                              BuyCount = 28
                          },


                          new Product
                          {
                              Id = Guid.Parse("04E7318D-61BB-455D-931B-4BAF081E8F99"),
                              Title = " DAREU EK87 - Black",
                              AuthorId = 10,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "SWD22EK87 ",
                              ListPrice = 25,
                              Price = 21,
                              Price50 = 15,
                              Price100 = 10,
                              CategoryId = 5,
                              CollectionId = 1,
                              BuyCount = 56

                          },
                          new Product
                          {
                              Id = Guid.Parse("9CBCC2E0-0DBF-437F-92C7-C18E96852A2D"),
                              Title = " EK810 Pink (USB/Pink Led/Blue/Brown/Red switch)  ",
                              AuthorId = 10,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "CAW57EEK810",
                              ListPrice = 35,
                              Price = 28,
                              Price50 = 19,
                              Price100 = 14,
                              CategoryId = 5,
                              CollectionId = 2,
                              BuyCount = 21


                          },
                          new Product
                          {
                              Id = Guid.Parse("3EF348A2-D648-4563-AA0E-E6569BC93D84"),
                              Title = " DAREU EK8100 100KEY (RGB, Blue/ Brown/ Red D switch)  ",
                              AuthorId = 10,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "RITOEK8100",
                              ListPrice = 43,
                              Price = 40,
                              Price50 = 35,
                              Price100 = 30,
                              CategoryId = 5,
                              CollectionId = 3,
                              BuyCount = 56

                          },
                          new Product
                          {
                              Id = Guid.Parse("091D512E-9706-4D9B-9823-58EB7F83F118"),
                              Title = "DAREU A87 SWALLOW (PBT, Brown/ Red CHERRY switch) ",
                              AuthorId = 10,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "XNDCA87SWALLOW",
                              ListPrice = 125,
                              Price = 121,
                              Price50 = 115,
                              Price100 = 110,
                              CategoryId = 5,
                              CollectionId = 4,
                              BuyCount = 66

                          },


                            new Product
                            {
                                Id = Guid.Parse("5B3D98F9-EFA7-4503-A9B1-FBDE14FC94DE"),
                                Title = " DAREU A918 - BLACK (PixArt PMW3335)",
                                AuthorId = 10,
                                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                                ISBN = "SWD24A918 ",
                                ListPrice = 30,
                                Price = 26,
                                Price50 = 19,
                                Price100 = 14,
                                CategoryId = 6,
                                CollectionId = 1,
                                BuyCount = 56

                            },
                          new Product
                          {
                              Id = Guid.Parse("38AED6B0-AB77-4D79-83FF-A9D3ADC46758"),
                              Title = " DareU EM901X RGB Superlight  ",
                              AuthorId = 10,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "CAW57EM901X",
                              ListPrice = 25,
                              Price = 21,
                              Price50 = 15,
                              Price100 = 10,
                              CategoryId = 6,
                              CollectionId = 2,
                              BuyCount = 77


                          },
                          new Product
                          {
                              Id = Guid.Parse("DD8D66E3-3D59-415C-8E82-E727B3E215BD"),
                              Title = " DAREU A960 YELLOW - ULTRALIGHT ",
                              AuthorId = 10,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "RITOA960YELLOW",
                              ListPrice = 43,
                              Price = 40,
                              Price50 = 35,
                              Price100 = 30,
                              CategoryId = 6,
                              CollectionId = 3,
                              BuyCount = 74

                          },
                          new Product
                          {
                              Id = Guid.Parse("457A2725-0870-4D56-9706-A5FD11A83FC1"),
                              Title = "Corsair M65 RGB ULTRA Black",
                              AuthorId = 11,
                              Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                              ISBN = "XNDCQM65SE",
                              ListPrice = 125,
                              Price = 121,
                              Price50 = 115,
                              Price100 = 110,
                              CategoryId = 6,
                              CollectionId = 4,
                              BuyCount = 22

                          });
                    #endregion

                    #region ProductImages

                    _db.ProductImages.AddRange(

                       new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\main\\a520m1.jpg", ProductId = Guid.Parse("33005AD9-DDD8-4DBB-B025-669D781F4B5F") },
                       new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\main\\a520m2.jpg", ProductId = Guid.Parse("33005AD9-DDD8-4DBB-B025-669D781F4B5F") },
                        new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\main\\b550m1.jpg", ProductId = Guid.Parse("4271654F-FA30-4A61-89BB-561B61D37EB7") },
                       new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\main\\b550m2.jpg", ProductId = Guid.Parse("4271654F-FA30-4A61-89BB-561B61D37EB7") },
                        new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\main\\x570tuf.jpg", ProductId = Guid.Parse("A180A819-51E9-46C9-B4E0-6E406A6FC658") },
                       new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\main\\x570tuf2.jpg", ProductId = Guid.Parse("A180A819-51E9-46C9-B4E0-6E406A6FC658") },
                            new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\main\\x570ace.jpg", ProductId = Guid.Parse("33ED75DE-1F7F-41AA-BB3D-FE2AB5F9D265") },
                       new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\main\\x570ace2.jpg", ProductId = Guid.Parse("33ED75DE-1F7F-41AA-BB3D-FE2AB5F9D265") },

                        new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\cpu\\3100.jpg", ProductId = Guid.Parse("88142B2B-D710-46C1-87B0-CA4FB9E8EE6E") },
                       new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\cpu\\3600.jpg", ProductId = Guid.Parse("8361D4AF-7BE2-4F20-B544-D54C1D149F90") },
                         new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\cpu\\5600.jpg", ProductId = Guid.Parse("EB5F667A-201A-4727-A367-EDC3F914B32D") },
                       new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\cpu\\5900.jpg", ProductId = Guid.Parse("B0AC6BBC-3C5A-4787-89AC-34B43B1EAA61") },

                   new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\monitor\\aoc32.jpg", ProductId = Guid.Parse("8B614A6D-52E9-4093-9948-00CCD45CA6D8") },
                      new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\monitor\\asus24vy.jpg", ProductId = Guid.Parse("B1317290-360B-4984-B45A-3B8138A11B39") },
                     new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\monitor\\edra24.jpg", ProductId = Guid.Parse("401DA5D4-6F94-44EA-A4C7-3489DB8056E8") },
                    new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\monitor\\hkcant.jpg", ProductId = Guid.Parse("C273F848-A068-42B4-9BAF-382079BB52A1") },

                     new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\vga\\550m.jpg", ProductId = Guid.Parse("380825B1-2650-4B48-8CC2-D9C35A509BA8") },
                      new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\vga\\550m2.jpg", ProductId = Guid.Parse("380825B1-2650-4B48-8CC2-D9C35A509BA8") },
                       new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\vga\\570m.jpg", ProductId = Guid.Parse("1DB6C932-E9EE-4FFD-BBF0-92EF23061982") },
                        new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\vga\\570m2.jpg", ProductId = Guid.Parse("1DB6C932-E9EE-4FFD-BBF0-92EF23061982") },
                         new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\vga\\rx6600xt1.jpg", ProductId = Guid.Parse("1FC51323-7607-4A7F-AAF7-05F09B0B3DFB") },
                          new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\vga\\rx6800xtm.jpg", ProductId = Guid.Parse("EA4D6B02-3EEE-430A-A160-E4D5F5C7C943") },

                            new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\keyboard\\dareua87.jpg", ProductId = Guid.Parse("091D512E-9706-4D9B-9823-58EB7F83F118") },
                            new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\keyboard\\dareuek810.jpg", ProductId = Guid.Parse("9CBCC2E0-0DBF-437F-92C7-C18E96852A2D") },
                            new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\keyboard\\ek87.jpg", ProductId = Guid.Parse("04E7318D-61BB-455D-931B-4BAF081E8F99") },
                            new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\keyboard\\ek8100.jpg", ProductId = Guid.Parse("3EF348A2-D648-4563-AA0E-E6569BC93D84") },

                            new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\mouse\\a918black.jpg", ProductId = Guid.Parse("5B3D98F9-EFA7-4503-A9B1-FBDE14FC94DE") },
                             new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\mouse\\a960y.jpg", ProductId = Guid.Parse("DD8D66E3-3D59-415C-8E82-E727B3E215BD") },
                              new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\mouse\\em901xm.jpg", ProductId = Guid.Parse("38AED6B0-AB77-4D79-83FF-A9D3ADC46758") },
                               new ProductImage { Id = Guid.NewGuid(), ImageUrl = "\\images\\product\\mouse\\m65rgb.jpg", ProductId = Guid.Parse("457A2725-0870-4D56-9706-A5FD11A83FC1") }
                   );
                }

                #endregion


                _db.SaveChanges();

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Đã xảy ra lỗi trong quá trình khởi tạo cơ sở dữ liệu:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

        }
    }
}
