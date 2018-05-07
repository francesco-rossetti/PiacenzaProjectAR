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

var crypto = require('crypto');

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

            var encpwd = crypto.createHash('md5').update(password).digest("hex");
            
        
            Request.addParameter("USR", Types.VarChar, username);
            Request.addParameter("PWD", Types.VarChar, encpwd);
        
            connection.execSql(Request);
        }
    });
};

server.register(Basic, function(err){
    if(err)
        throw err;
});

server.auth.strategy('simple', 'basic', {validateFunc: validate});

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
    path: "/api/getURL",
    handler:function(request, reply){
        var Response = [];
        var connection = new Connection(config);

        connection.on("connect", function(err){
            if(err)
                reply({ status: "ko", result: err });
            else
            {
                var idmon = null;
                var Request = new Requests("EXEC GETURL @ID", function(err, rowcount){
                    var Response = [];
                    if(err)
                        reply({ status: "ko", result: err });
                    else
                    {
                        if(rowcount == 0)
                            reply({ status: "ok", result: "Err005" });
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
                            reply({ status: "ok", result: [{ key: "field_1", value: "Err003"}, { key: "field_2", value: "Err003"}, { key: "field_3", value: "Err003"}] });
                        else
                            reply({ status: "ok", result: fid });
                    }
                });

                Request.on("row", function(col){
                    var Row = {};
                    col.forEach(function(elem){
                        if(elem.metadata.colName == "FIELD")
                            Row["key"] = "field_" + elem.value;
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

server.route({
    method: "POST",
    path: "/api/insertMuseum",
    config: {
        auth: 'simple',
        handler:function(request, reply){
            var connection = new Connection(config);
    
            connection.on("connect", function(err){
                if(err)
                    reply({ Field: { status: "ko", result: err }});
                else
                {
                    var fid = [];
                    var Request = new Requests("INSERT INTO MONUMENTO (TITLE) VALUES(@DESC)", function(err, rowcount){
                        if(err)
                            reply({ status: "ko", result: err });
                        else
                        {
                            if(rowcount == 0)
                                reply({ status: "ok", result: "Err004" });
                            else
                                reply({ status: "ok", result: "ok" });
                        }
                    });
            
                    Request.addParameter("DESC", Types.VarChar, request.payload.title);
                
                    connection.execSql(Request);
                }
            });
        }
    }
});

server.route({
    method: "POST",
    path: "/api/updateMuseum",
    config: {
        auth: 'simple',
        handler:function(request, reply){
            var connection = new Connection(config);
    
            connection.on("connect", function(err){
                if(err)
                    reply({ Field: { status: "ko", result: err }});
                else
                {
                    var fid = [];
                    var Request = new Requests("UPDATE MONUMENTO SET TITLE = @DESC WHERE IDMONUMENTO = @ID", function(err, rowcount){
                        if(err)
                            reply({ status: "ko", result: err });
                        else
                        {
                            if(rowcount == 0)
                                reply({ status: "ok", result: "Err004" });
                            else
                                reply({ status: "ok", result: "ok" });
                        }
                    });
            
                    Request.addParameter("DESC", Types.VarChar, request.payload.title);
                    Request.addParameter("ID", Types.Int, request.payload.idmon);
                
                    connection.execSql(Request);
                }
            });
        }
    }
});

server.route({
    method: "POST",
    path: "/api/deleteMuseum",
    config: {
        auth: 'simple',
        handler:function(request, reply){
            var connection = new Connection(config);
    
            connection.on("connect", function(err){
                if(err)
                    reply({ Field: { status: "ko", result: err }});
                else
                {
                    var fid = [];
                    var Request = new Requests("DELETE FROM MONUMENTO WHERE IDMONUMENTO = @ID", function(err, rowcount){
                        if(err)
                            reply({ status: "ko", result: err });
                        else
                        {
                            if(rowcount == 0)
                                reply({ status: "ok", result: "Err004" });
                            else
                                reply({ status: "ok", result: "ok" });
                        }
                    });
            
                    Request.addParameter("ID", Types.Int, request.payload.idmon);
                
                    connection.execSql(Request);
                }
            });
        }
    }
});

server.route({
    method: "POST",
    path: "/api/insertDetails",
    config: {
        auth: 'simple',
        handler:function(request, reply){
            var connection = new Connection(config);
    
            connection.on("connect", function(err){
                if(err)
                    reply({ Field: { status: "ko", result: err }});
                else
                {
                    var fid = [];
                    var Request = new Requests("INSERT INTO DESCRIZIONE (DESCRIZIONE, FIELD, LANG, IDMONUMENTO) VALUES(@DESC, @FIELD, @LANG, @IDM)", function(err, rowcount){
                        if(err)
                            reply({ status: "ko", result: err });
                        else
                        {
                            if(rowcount == 0)
                                reply({ status: "ok", result: "Err004" });
                            else
                                reply({ status: "ok", result: "ok" });
                        }
                    });
            
                    Request.addParameter("DESC", Types.Text, request.payload.descrizione);
                    Request.addParameter("FIELD", Types.Int, request.payload.field);
                    Request.addParameter("LANG", Types.VarChar, request.payload.lang);
                    Request.addParameter("IDM", Types.Int, request.payload.idmon);
                
                    connection.execSql(Request);
                }
            });
        }
    }
    
});

server.route({
    method: "POST",
    path: "/api/updateDetails",
    config: {
        auth: 'simple',
        handler:function(request, reply){
            var connection = new Connection(config);
    
            connection.on("connect", function(err){
                if(err)
                    reply({ Field: { status: "ko", result: err }});
                else
                {
                    var fid = [];
                    var Request = new Requests("UPDATE DESCRIZIONE SET DESCRIZIONE = @DESC, FIELD = @FIELD, LANG = @LANG, IDMONUMENTO = @IDM WHERE IDESCRIZIONE = @ID", function(err, rowcount){
                        if(err)
                            reply({ status: "ko", result: err });
                        else
                        {
                            if(rowcount == 0)
                                reply({ status: "ok", result: "Err004" });
                            else
                                reply({ status: "ok", result: "ok" });
                        }
                    });
            
                    Request.addParameter("DESC", Types.Text, request.payload.descrizione);
                    Request.addParameter("FIELD", Types.Int, request.payload.field);
                    Request.addParameter("LANG", Types.VarChar, request.payload.lang);
                    Request.addParameter("IDM", Types.Int, request.payload.idmon);
                    Request.addParameter("ID", Types.Int, request.payload.idesc);
                
                    connection.execSql(Request);
                }
            });
        }
    }
    
});

server.route({
    method: "POST",
    path: "/api/deleteDetails",
    config: {
        auth: 'simple',
        handler:function(request, reply){
            var connection = new Connection(config);
    
            connection.on("connect", function(err){
                if(err)
                    reply({ Field: { status: "ko", result: err }});
                else
                {
                    var fid = [];
                    var Request = new Requests("DELETE FROM DESCRIZIONE WHERE IDESCRIZIONE = @ID", function(err, rowcount){
                        if(err)
                            reply({ status: "ko", result: err });
                        else
                        {
                            if(rowcount == 0)
                                reply({ status: "ok", result: "Err004" });
                            else
                                reply({ status: "ok", result: "ok" });
                        }
                    });

                    Request.addParameter("ID", Types.Int, request.payload.idesc);
                
                    connection.execSql(Request);
                }
            });
        }
    }
    
});

/* CONFIGURAZIONE SERVER */

server.start(function(err){
    if(err)
        console.log(err);
    else
        console.log('Server Started');
});