// Define una interfaz para la estructura de la factura.
interface Factura {
  numero: string;
  cliente: string;
  total: number;
  fecha: string;
}

// Define un alias de tipo para el array de facturas.
type FacturasResponse = Factura[];

class ProcesadorFacturas {
  // Las facturas deben ser una propiedad de la clase, no una variable local en un método.
  // Esto permite que la clase sea más flexible y reutilizable.
  private facturas: FacturasResponse = [
    {
      numero: "F001",
      cliente: "Comercial S.A.",
      total: 1200.5,
      fecha: "2025-07-01",
    },
    {
      numero: "F002",
      cliente: "Insumos del Litoral",
      total: 450.0,
      fecha: "2025-07-03",
    },
    {
      numero: "F003",
      cliente: "Ferretería Norte",
      total: 2300.75,
      fecha: "2025-07-02",
    },
    {
      numero: "F004",
      cliente: "Constructora Andes",
      total: 800.0,
      fecha: "2025-07-05",
    },
    {
      numero: "F005",
      cliente: "Servicios Eléctricos",
      total: 1575.3,
      fecha: "2025-07-04",
    },
    {
      numero: "F006",
      cliente: "Comercial S.A.",
      total: 600.0,
      fecha: "2025-07-06",
    },
    {
      numero: "F007",
      cliente: "Ferretería Norte",
      total: 3400.1,
      fecha: "2025-07-07",
    },
    {
      numero: "F008",
      cliente: "Insumos del Litoral",
      total: 980.5,
      fecha: "2025-07-08",
    },
    {
      numero: "F009",
      cliente: "Servicios Eléctricos",
      total: 2100.0,
      fecha: "2025-07-09",
    },
    {
      numero: "F010",
      cliente: "Constructora Andes",
      total: 300.0,
      fecha: "2025-07-10",
    },
  ];

  /**
   * Filtra, ordena y limita las facturas por cliente.
   * @param cliente El nombre del cliente a buscar.
   * @param numeroDeFacturas El número de facturas a devolver. Por defecto es 5.
   * @returns FacturasResponse Un array de las facturas filtradas y ordenadas.
   */
  public obtenerFacturasPorCliente(
    cliente: string,
    numeroDeFacturas: number = 5
  ): FacturasResponse {
    // Es una buena práctica usar una copia del array original para evitar mutarlo.
    const facturasCopia = [...this.facturas];
    // Filtrar las factura por el cliente indicado. Incluyendo el uso me mayusculas y minusculas.
    const facturasFiltradas = facturasCopia.filter(
      (factura) =>
        factura.cliente.trim().toLowerCase() === cliente.trim().toLowerCase()
    );
    // Ordenar las facturas previamente filtradas por el orden descendiente según su total
    const facturasOrdenadas = facturasFiltradas.sort(
      (a, b) => b.total - a.total
    ); // El segundo parámetro es el que va primero para el orden descendente, en caso de ser ascendente el primer parámetro va primero

    // Devolver el array acorde al numero indicado, encaso de no establecer otro valor, se devuelven  5
    return facturasOrdenadas.slice(0, numeroDeFacturas);
  }
}

// --- Uso de la clase ---
// 1 Creamos la instancia de la clase
const procesador = new ProcesadorFacturas();
// 2. Definimos los parámetros de entrada
const clienteSeleccionado = "Constructora Andes";
const limiteFacturas = 5;

// 3. Mediante la instancia, llamamos a la función y guardamos el resultado de la busqueda
const resultado = procesador.obtenerFacturasPorCliente(
  clienteSeleccionado,
  limiteFacturas
);
// 4. Logueamos el resultado mediante console.log y console.table
console.log(
  `Las ${resultado.length} facturas con mayor monto para ${clienteSeleccionado} son:`
);

console.table(resultado); // Este imprime una tabla 
