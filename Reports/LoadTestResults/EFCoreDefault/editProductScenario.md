> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `Edit Product`, duration: `00:00:30`, ok count: `55794`, fail count: `0`, all data: `0` MB MB

load simulation: `keep_constant`, copies: `30`, during: `00:00:30`
|step|ok stats|
|---|---|
|name|`editProduct`|
|request count|all = `55794`, ok = `55794`, RPS = `1859,8`|
|latency|min = `8,67`, mean = `16,07`, max = `43,63`, StdDev = `2,6`|
|latency percentile|50% = `15,82`, 75% = `17,36`, 95% = `20,26`, 99% = `25,3`|
> status codes for scenario: `Edit Product`

|status code|count|message|
|---|---|---|
|200|55794||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|Edit Product|Step 'editProduct' in scenario 'Edit Product' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
