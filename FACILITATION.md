# Facilitation

## Objetivo de la sesion

Guiar a grupos de 2 o 3 personas para aplicar parallel change en una API .NET sin romper compatibilidad.

## Duracion sugerida

- 15 min contexto y reglas
- 55 min hands-on guiado
- 20 min cierre tecnico y debrief

## Reglas durante el hands-on

- empezar siempre desde `workshop/initial-state`
- un test en rojo por vez
- minima implementacion para verde
- refactor inmediato
- commit pequeno cuando todo esta en verde

## Checkpoints

1. `v1` estable y entendido
2. `v2` coexistiendo con `v1`
3. dual write visible y explicado
4. backfill por lotes y point of no easy return
5. contract final con `v2` only

## Dinamica por equipo

- rotar driver y navigator cada 15 minutos
- el navigator vigila naming, test quality y alcance del commit
- no avanzar de fase con tests en rojo

## Mensajes clave de teoria

- el riesgo no es el cambio tecnico aislado, es la coordinacion
- expand crea opcionalidad
- migrate mueve consumidores y datos sin corte brusco
- contract elimina deuda transicional y devuelve claridad

## Criterio de exito de la sesion

- el equipo sabe explicar por que no hay breaking change
- el equipo puede identificar el point of no easy return
- el equipo entrega una rama verde y coherente con la fase alcanzada
