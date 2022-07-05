> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `Edit Product`, duration: `00:00:30`, ok count: `74161`, fail count: `0`, all data: `0` MB MB

load simulation: `keep_constant`, copies: `30`, during: `00:00:30`
|step|ok stats|
|---|---|
|name|`editProduct`|
|request count|all = `74161`, ok = `74161`, RPS = `2472`|
|latency|min = `5,62`, mean = `12,09`, max = `30,28`, StdDev = `2,37`|
|latency percentile|50% = `11,74`, 75% = `13,23`, 95% = `16,3`, 99% = `20,35`|
> status codes for scenario: `Edit Product`

|status code|count|message|
|---|---|---|
|200|74161||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|Edit Product|Step 'editProduct' in scenario 'Edit Product' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
