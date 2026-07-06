using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using RestaurantManagerMVC.Models;

namespace RestaurantManagerMVC.Services
{
    public class ExportService
    {
        public ExportService()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }


        public byte[] RestoranlarPdf(List<Restoran> liste)
        {
            var basliklar = new[] { "Id", "Ad", "Adres", "Telefon", "Sehir" };
            var satirlar = liste.Select(r => new[]
            {
                r.Id.ToString(), r.Ad, r.Adres ?? "-", r.Telefon ?? "-", r.Sehir ?? "-"
            });
            return GenericPdf("Restoran Listesi", basliklar, satirlar);
        }

        public byte[] YemeklerPdf(List<Yemek> liste)
        {
            var basliklar = new[] { "Id", "Yemek", "Restoran", "Kategori", "Fiyat" };
            var satirlar = liste.Select(y => new[]
            {
                y.Id.ToString(), y.Ad, y.RestoranAdi ?? "-", y.Kategori ?? "-", y.Fiyat.ToString("N2") + " TL"
            });
            return GenericPdf("Yemek Listesi", basliklar, satirlar);
        }

        public byte[] SiparislerPdf(List<Siparis> liste)
        {
            var basliklar = new[] { "Id", "Musteri", "Restoran", "Yemek", "Adet", "Tarih", "Durum" };
            var satirlar = liste.Select(s => new[]
            {
                s.Id.ToString(), s.MusteriAdi, s.RestoranAdi ?? "-", s.YemekAdi ?? "-",
                s.Adet.ToString(), s.Tarih.ToString("dd.MM.yyyy HH:mm"), s.Durum
            });
            return GenericPdf("Siparis Listesi", basliklar, satirlar);
        }

        private byte[] GenericPdf(string baslik, string[] basliklar, IEnumerable<string[]> satirlar)
        {
            using var stream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Text(baslik).FontSize(18).Bold();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            foreach (var _ in basliklar)
                                columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            foreach (var b in basliklar)
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text(b).Bold();
                            }
                        });

                        foreach (var satir in satirlar)
                        {
                            foreach (var hucre in satir)
                            {
                                table.Cell().Padding(4).Text(hucre);
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Olusturulma: ");
                        x.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                    });
                });
            }).GeneratePdf(stream);

            return stream.ToArray();
        }


        public byte[] RestoranlarExcel(List<Restoran> liste)
        {
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Restoranlar");

            string[] basliklar = { "Id", "Ad", "Adres", "Telefon", "Sehir" };
            for (int i = 0; i < basliklar.Length; i++)
                sheet.Cells[1, i + 1].Value = basliklar[i];

            for (int i = 0; i < liste.Count; i++)
            {
                var r = liste[i];
                var satir = i + 2;
                sheet.Cells[satir, 1].Value = r.Id;
                sheet.Cells[satir, 2].Value = r.Ad;
                sheet.Cells[satir, 3].Value = r.Adres;
                sheet.Cells[satir, 4].Value = r.Telefon;
                sheet.Cells[satir, 5].Value = r.Sehir;
            }

            StyleHeader(sheet, basliklar.Length);
            sheet.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }

        public byte[] YemeklerExcel(List<Yemek> liste)
        {
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Yemekler");

            string[] basliklar = { "Id", "Yemek", "Restoran", "Kategori", "Fiyat" };
            for (int i = 0; i < basliklar.Length; i++)
                sheet.Cells[1, i + 1].Value = basliklar[i];

            for (int i = 0; i < liste.Count; i++)
            {
                var y = liste[i];
                var satir = i + 2;
                sheet.Cells[satir, 1].Value = y.Id;
                sheet.Cells[satir, 2].Value = y.Ad;
                sheet.Cells[satir, 3].Value = y.RestoranAdi;
                sheet.Cells[satir, 4].Value = y.Kategori;
                sheet.Cells[satir, 5].Value = y.Fiyat;
            }

            StyleHeader(sheet, basliklar.Length);
            sheet.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }

        public byte[] SiparislerExcel(List<Siparis> liste)
        {
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Siparisler");

            string[] basliklar = { "Id", "Musteri", "Restoran", "Yemek", "Adet", "Tarih", "Durum" };
            for (int i = 0; i < basliklar.Length; i++)
                sheet.Cells[1, i + 1].Value = basliklar[i];

            for (int i = 0; i < liste.Count; i++)
            {
                var s = liste[i];
                var satir = i + 2;
                sheet.Cells[satir, 1].Value = s.Id;
                sheet.Cells[satir, 2].Value = s.MusteriAdi;
                sheet.Cells[satir, 3].Value = s.RestoranAdi;
                sheet.Cells[satir, 4].Value = s.YemekAdi;
                sheet.Cells[satir, 5].Value = s.Adet;
                sheet.Cells[satir, 6].Value = s.Tarih.ToString("dd.MM.yyyy HH:mm");
                sheet.Cells[satir, 7].Value = s.Durum;
            }

            StyleHeader(sheet, basliklar.Length);
            sheet.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }

        private static void StyleHeader(ExcelWorksheet sheet, int kolonSayisi)
        {
            using var range = sheet.Cells[1, 1, 1, kolonSayisi];
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }
    }
}
