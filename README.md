# Food Delivery Microservice

<h2>About</h2>
<p>A food delivery website using microservice architeture.</p>
<p>Custom Logs using <a href="https://serilog.net" target="_blank">Serilog</a>.</p>
<p>Documented using <a href="https://swagger.io" target="_blank">Swagger UI</a>.</p>

<h4>Json Web Token (JWT)</h4>
<p>
  The JSON Web Token (<a href="https://jwt.io" target="_blank">JWT</a>) is an Internet standard for creating data with optional 
  signature and/or encryption whose payload contains JSON that asserts some number of claims. Tokens are signed using a private 
  secret or a public/private key.
</p>


<h4>SignalR</h4>
<p>
    <a href="https://dotnet.microsoft.com/apps/aspnet/signalr" target="_blank">SignalR</a> is a free and open-source software 
    library for Microsoft ASP.NET that allows server code to send asynchronous notifications to client-side web applications. 
    The library includes server-side and client-side JavaScript components.
</p>
<p>
    Like the rest of ASP.NET, <a href="https://dotnet.microsoft.com/apps/aspnet/signalr" target="_blank">SignalR</a> was built 
    for high performance and is one of the fastest real-time frameworks around.
    Scale out across servers with built-in support for using <a href="https://redis.io/" target="_blank">Redis</a>, SQL Server, or Azure Service Bus 
    to coordinate messages between each instance.
</p>

<h4>Redis</h4>
<p>
    <a href="https://redis.io/" target="_blank">Redis</a> is an open source (BSD licensed), in-memory data structure store, used as a database, 
    cache, and message broker. 
    <a href="https://redis.io/" target="_blank">Redis</a> provides data structures such as strings, hashes, lists, sets, sorted sets with 
    range queries, bitmaps, hyperloglogs, geospatial indexes, and streams.
</p>

<h4>Docker Hub</h4>
<p>
    <a href="https://www.docker.com/" target="_blank">Docker</a> is a set of platform-as-a-service products that use operating system-level 
    virtualization to deliver software in packages called containers.
    Containers are granted to others and group their own software, libraries and configuration files.
</p>

<h4>Microsoft Identity</h4>
<p>
    The <a href="https://docs.microsoft.com/en-us/azure/active-directory/develop/" target="_target">Microsoft Identity</a> platform helps you build applications
    your users and customers can sign in to using their Microsoft identities or social accounts, and provide authorized access to your own APIs or 
    Microsoft APIs like Microsoft Graph.
</p>

<h4>Microsoft SQL Server</h4>
<p>
    <a href="https://www.microsoft.com/en-us/sql-server/sql-server-2019" target="_blank">Microsoft SQL Server</a> is a relational database management 
    system developed by Sybase in partnership with Microsoft. 
    This partnership lasted until 1994, with the release of the version for Windows NT and since then Microsoft maintains the maintenance of the product.
</p>

<h4>Swagger</h4>
<p>
    <a href="https://swagger.io/" target="_blank">Swagger</a> is an Interface Description Language for describing RESTful APIs expressed using JSON. 
    <a href="https://swagger.io/" target="_blank">Swagger</a> is used together with a set of open-source software tools to design, build, document, 
    and use RESTful web services. 
    <a href="https://swagger.io/" target="_blank">Swagger</a> includes automated documentation, code generation, and test-case generation.
</p>

<h2>Tools</h2>
<ul>
    <li>.NET 5.0+</li>
    <li>VS Code</li>
    <li><a href="https://www.microsoft.com/en-us/sql-server/sql-server-2019" target="_blank">Microsoft SQL Server</a></li>
    <li><a href="https://www.docker.com/" target="_blank">Docker</a> (Optional)</li>
    <li><a href="https://redis.io/">Redis</a></li>
</ul>


<h2>How to Run</h2>
<ol>
    <li>Run <code>dotnet restore</code></li>
    <li>Change the ConnectionString in appsettings.json</li>
    <li>Run <code>dotnet ef migrations add {migration name}</code></li>
    <li>Run <code>dotnet ef database update</code></li>
    <li>Run <code>Redis</code> (if you're not using docker)</li>
    <li>Install <a href="https://hub.docker.com/_/redis" target="_blank">Redis Docker image</a> typing <code>docker pull redis</code> (if you are using Docker)</li>
    <li>Run Redis container typing <code>docker run --name some-redis -d redis redis-server --appendonly yes</code></li>
    <li>Run <code>dotnet run</code></li>
</ol>
