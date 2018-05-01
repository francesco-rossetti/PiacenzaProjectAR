var config = {
    userName: "sa",
    password: ".",
    server: "127.0.0.1",
    options: {
        database: "PROJECTAR"
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
    method: "POST",
    path: "/api/getMonumentName",
    handler:function(request, reply){
        var Response = [];
        var connection = new Connection(config);

        connection.on("connect", function(err){
            if(err)
                reply([{ status: "ko", result: "Err002" }]);
            else
            {
                var idmon = null;
                var Request = new Requests("EXEC GETMONUMENTITILE @ID", function(err, rowcount){
                    var Response = [];
                    if(err)
                        Response.push({ status: "ko", result: err });
                    else
                    {
                        if(rowcount == 0)
                            Response.push({ status: "ok", result: "Err002" });
                        else
                            Response.push({ status: "ok", result: idmon});
                    }
        
                    reply(Response);
                });

                Request.on("row", function(col){
                    col.forEach(function(elem){
                        idmon = elem.value;
                    });
                });
        
                Request.addParameter("ID", Types.Int, request.payload.idmon);
            
                connection.execSql(Request);
            }
        });
    }
});

server.route({
    method: "POST",
    path: "/api/getField",
    handler:function(request, reply){
        var Response = [];
        var connection = new Connection(config);

        connection.on("connect", function(err){
            if(err)
                reply([{ status: "ko", result: "Err003" }]);
            else
            {
                var fid = [];
                var Request = new Requests("EXEC GETARRAYVALORI @ID, @LANG", function(err, rowcount){
                    var Response = [];
                    if(err)
                        Response.push({ status: "ko", result: err });
                    else
                    {
                        if(rowcount == 0)
                            Response.push({ status: "ok", result: "Err003" });
                        else
                            Response.push({ status: "ok", result: fid});
                    }
        
                    reply(Response);
                });

                Request.on("row", function(col){
                    var Row = {};
                    var fieid = -1;
                    col.forEach(function(elem){
                        if(elem.metadata.colName == "FIELD")
                            fieid = elem.value;
                        else
                            Row["field_" + fieid] = elem.value;
                    });

                    fid.push(Row);
                });
        
                Request.addParameter("ID", Types.Int, request.payload.idmon);
                Request.addParameter("LANG", Types.VarChar, request.payload.lang);
            
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