var config = {
    userName: "Rossetti",
    password: "password.123",
    server: "rossettiesercitazioni.database.windows.net",
    options: {
        database: "projectAR",
        encrypt: true
    }
};

const Hapi = require("hapi");
const Basic = require("hapi-auth-basic");

var Connection = require("tedious").Connection;
var Requests = require("tedious").Request;
var Types = require("tedious").TYPES;

const server = new Hapi.Server();

server.connection({
    host: process.env.HOST || 'localhost',
    port: process.env.PORT || 8080
});

/* AUTH */

const validate = function(request, username, password, callback){
    var connection = new Connection(config);

    connection.on("connect", function(err){
        if(err)
            reply([{ status: "ko", result: "Err004" }]);
        else
        {
            var Request = new Requests("SELECT IDUTENTE FROM UTENTE WHERE USR = @USR AND PWD = @PWD", function(err, rowcount){
                var Response = [];
                if(err)
                    return callback(null, false);
                else
                {
                    if(rowcount == 0)
                        return callback(null, false);
                    else
                        return callback(null, true, {username: username, password: password});
                }
            });
        
            Request.addParameter("USR", Types.VarChar, username);
            Request.addParameter("PWD", Types.VarChar, password);
        
            connection.execSql(Request);
        }
    });
};

/* ROUTE SERVER */

server.route({
    method: "GET",
    path: "/api/getMonumentName",
    handler:function(request, reply){
        var Response = [];
        var connection = new Connection(config);

        connection.on("connect", function(err){
            if(err)
                reply({ status: "ko", result: err });
            else
            {
                var idmon = null;
                var Request = new Requests("EXEC GETMONUMENTITILE @ID", function(err, rowcount){
                    var Response = [];
                    if(err)
                        reply({ status: "ko", result: err });
                    else
                    {
                        if(rowcount == 0)
                            reply({ status: "ok", result: "Err002" });
                        else
                            reply({ status: "ok", result: idmon });
                    }
                });

                Request.on("row", function(col){
                    col.forEach(function(elem){
                        idmon = elem.value;
                    });
                });
        
                Request.addParameter("ID", Types.Int, request.query.idmon);
            
                connection.execSql(Request);
            }
        });
    }
});

server.route({
    method: "GET",
    path: "/api/getField",
    handler:function(request, reply){
        var connection = new Connection(config);

        connection.on("connect", function(err){
            if(err)
                reply({ Field: { status: "ko", result: err }});
            else
            {
                var fid = [];
                var Request = new Requests("EXEC GETARRAYVALORI @ID, @LANG", function(err, rowcount){
                    if(err)
                        reply({ status: "ko", result: err });
                    else
                    {
                        if(rowcount == 0)
                            reply({ status: "ok", result: "Err003" });
                        else
                            reply({ status: "ok", result: fid });
                    }
                });

                Request.on("row", function(col){
                    var Row = {};
                    col.forEach(function(elem){
                        if(elem.metadata.colName == "FIELD")
                            Row["key"] = elem.value;
                        else
                            Row["value"] = elem.value;
                    });

                    fid.push(Row);
                });
        
                Request.addParameter("ID", Types.Int, request.query.idmon);
                Request.addParameter("LANG", Types.VarChar, request.query.lang);
            
                connection.execSql(Request);
            }
        });
    }
});



/* CONFIGURAZIONE SERVER */

server.register(Basic, function(err){
    if(err)
        throw err;
});

server.auth.strategy('simple', 'basic', {validateFunc: validate});

server.start(function(err){
    if(err)
        console.log(err);
    else
        console.log('Server Started');
});