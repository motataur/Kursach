using System;
using System.Collections.Generic;
using System.Linq;
using Kursach.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Kursach.DB;

public static class ReportGenerator
{
    public static byte[] GenerateRentReport(List<RentReportRow> rows, DateTime from, DateTime to)
    {
        var totalIncome = rows.Sum(r => r.Price);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Column(col =>
                {
                    col.Item().Text("Отчёт о заселениях").FontSize(20).Bold();
                    col.Item().Text($"Период: {from:dd.MM.yyyy} — {to:dd.MM.yyyy}").FontSize(12);
                });

                page.Content().PaddingTop(15).Column(col =>
                {
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.ConstantColumn(60);
                            c.RelativeColumn(2);
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCell).Text("Комната");
                            header.Cell().Element(HeaderCell).Text("Клиент");
                            header.Cell().Element(HeaderCell).Text("Заселение");
                            header.Cell().Element(HeaderCell).Text("Выселение");
                            header.Cell().Element(HeaderCell).Text("Сумма");
                        });

                        foreach (var row in rows)
                        {
                            table.Cell().Element(BodyCell).Text(row.RoomNumber.ToString());
                            table.Cell().Element(BodyCell).Text(row.ClientFullName);
                            table.Cell().Element(BodyCell).Text(row.StartDate.ToString("dd.MM.yyyy"));
                            table.Cell().Element(BodyCell).Text(row.EndDate.ToString("dd.MM.yyyy"));
                            table.Cell().Element(BodyCell).Text($"{row.Price} ₽");
                        }
                    });

                    col.Item().PaddingTop(20).AlignRight().Text($"Всего заселений: {rows.Count}").FontSize(12);
                    col.Item().AlignRight().Text($"Общий доход: {totalIncome} ₽").FontSize(14).Bold();
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Сформировано: ");
                    text.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                });
            });
        });

        return document.GeneratePdf();
    }

    private static IContainer HeaderCell(IContainer container)
    {
        return container
            .Background(Colors.Grey.Lighten2)
            .Padding(5)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Darken1)
            .DefaultTextStyle(x => x.Bold());
    }

    private static IContainer BodyCell(IContainer container)
    {
        return container
            .Padding(5)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten1);
    }
}