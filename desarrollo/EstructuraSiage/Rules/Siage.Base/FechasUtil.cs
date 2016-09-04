using System;

namespace Siage.Base
{
    public class FechasUtil
    {
        public static DateTime[,] CalcularPeriodosMensuales(DateTime fechaDesde, DateTime fechaHasta)
        {
            DateTime[,] periodos = null;

            if (fechaDesde.Year == fechaHasta.Year)
            {
                if (fechaDesde.Month == fechaHasta.Month)
                {
                    periodos = new DateTime[1, 2];
                    periodos[0, 0] = fechaDesde;
                    periodos[0, 1] = fechaHasta;
                }
                else
                {
                    int cantMeses = fechaHasta.Month - fechaDesde.Month;
                    periodos = new DateTime[cantMeses + 1, 2];

                    var finMes1 = DateTime.DaysInMonth(fechaDesde.Year, fechaDesde.Month);

                    periodos[0, 0] = fechaDesde;
                    periodos[0, 1] = new DateTime(fechaDesde.Year, fechaDesde.Month, finMes1);

                    for (int i = 1; i <= cantMeses; i++)
                    {
                        var mes = fechaDesde.Month + i;
                        periodos[i, 0] = new DateTime(fechaDesde.Year, mes, 1);

                        if (fechaHasta.Month == mes)
                            periodos[i, 1] = fechaHasta;
                        else
                            periodos[i, 1] = new DateTime(fechaDesde.Year, mes, DateTime.DaysInMonth(fechaDesde.Year, mes));
                    }
                }
            }
            else
            {
                int cantMeses = 0;
                cantMeses += 12 - fechaDesde.Month;
                cantMeses += fechaHasta.Month;

                int cantAnios = fechaHasta.Year - fechaDesde.Year;
                if (cantAnios > 1)
                    cantMeses += 12 * (cantAnios - 1);

                periodos = new DateTime[cantMeses + 1, 2];

                // Primer periodo
                var finMes1 = DateTime.DaysInMonth(fechaDesde.Year, fechaDesde.Month);

                periodos[0, 0] = fechaDesde;
                periodos[0, 1] = new DateTime(fechaDesde.Year, fechaDesde.Month, finMes1);

                // Periodos intermedios
                int mes = fechaDesde.Month + 1;
                int anio = fechaDesde.Year;
                int periodo = 1;

                while (anio <= fechaHasta.Year)
                {
                    if (mes > 12)
                    {
                        mes = 1;
                        anio++;
                    }

                    periodos[periodo, 0] = new DateTime(anio, mes, 1);

                    // Ultimo periodo
                    if (anio == fechaHasta.Year && mes == fechaHasta.Month)
                    {
                        periodos[periodo, 1] = fechaHasta;
                        break;
                    }

                    periodos[periodo, 1] = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));

                    periodo++;
                    mes++;
                }
            }

            return periodos;
        }
    }
}
