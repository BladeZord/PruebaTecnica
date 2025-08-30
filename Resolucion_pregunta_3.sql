/*
PLANTEAMIENTO:
Dada la consulta SELECT * FROM cabeceraventas WHERE CODIGOCLIENTE = '123' ORDER BY FECHA DESC;
Optimizar y explicar qué índices o cambios aplicarías para mejorar rendimiento.

*/
-- Al existir una consulta que filtra por CODIGOCLIENTE y ordena por FECHA,
-- se recomienda crear un índice compuesto que incluya ambas columnas.
CREATE INDEX idx_cliente_fecha 
ON cabeceraventas (CODIGOCLIENTE, FECHA DESC);
-- Este índice permitirá que la base de datos utilice el índice para filtrar por CODIGOCLIENTE
-- y luego ordenar los resultados por FECHA de manera eficiente, mejorando el rendimiento de la consulta.   

-- Otra opcion es que se sugiere que en vez de ejecutar el SELECT * se especifiquen solo las columnas necesarias
-- Como el siguiente ejemplo:
SELECT CODIGOCLIENTE, FECHA, TOTAL 
FROM cabeceraventas
WHERE CODIGOCLIENTE = '123'
ORDER BY FECHA DESC;


-- Tambien esta la particularidad de tratar de limiar la cantidad de resultados que se obtienen
SELECT CODIGOCLIENTE, FECHA, TOTAL
FROM cabeceraventas
WHERE CODIGOCLIENTE = '123'
ORDER BY FECHA DESC
LIMIT 10;
