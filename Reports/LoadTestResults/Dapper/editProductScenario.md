> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `Edit Product`, duration: `00:00:30`, ok count: `131152`, fail count: `0`, all data: `0` MB MB

load simulation: `keep_constant`, copies: `30`, during: `00:00:30`
|step|ok stats|
|---|---|
|name|`editProduct`|
|request count|all = `131152`, ok = `131152`, RPS = `4371,7`|
|latency|min = `2,59`, mean = `6,82`, max = `22,33`, StdDev = `1,89`|
|latency percentile|50% = `6,44`, 75% = `7,92`, 95% = `10,34`, 99% = `12,29`|
> status codes for scenario: `Edit Product`

|status code|count|message|
|---|---|---|
|200|131152||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|Edit Product|Step 'editProduct' in scenario 'Edit Product' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
