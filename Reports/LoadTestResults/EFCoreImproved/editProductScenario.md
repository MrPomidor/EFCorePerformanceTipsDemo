> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `Edit Product`, duration: `00:00:30`, ok count: `60028`, fail count: `0`, all data: `0` MB MB

load simulation: `keep_constant`, copies: `30`, during: `00:00:30`
|step|ok stats|
|---|---|
|name|`editProduct`|
|request count|all = `60028`, ok = `60028`, RPS = `2000,9`|
|latency|min = `7,46`, mean = `14,94`, max = `35,78`, StdDev = `2,67`|
|latency percentile|50% = `14,56`, 75% = `16,15`, 95% = `19,87`, 99% = `24,37`|
> status codes for scenario: `Edit Product`

|status code|count|message|
|---|---|---|
|200|60028||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|Edit Product|Step 'editProduct' in scenario 'Edit Product' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
