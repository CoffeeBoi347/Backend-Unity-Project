import flask
import uuid

app = flask.Flask(__name__)

users = {}
tokens = {}
deleted_users = {}

@app.route("/register", methods=["POST"])
def registerUser():
    data = flask.request.get_json()
    username = data.get("username")
    password = data.get("password")

    if not username or not password:
        return flask.jsonify({"error":"No credentials found for username and password."}), 400
    
    if username in users:
        return flask.jsonify({"error":"User already exists!"}), 409
    
    users[username] = password
    return flask.jsonify({"status":"OK."}), 200

@app.route("/login", methods=["POST"])
def loginUser():
    data = flask.request.get_json()
    username = data.get("username")
    password = data.get("password")

    if not username or not password:
        return flask.jsonify({"error":"No credentials found for username and password."}), 400
    
    if username not in users:
        return flask.jsonify({"error":"User does not exist."}), 404
    
    if(users.get(username) != password):
        return flask.jsonify({"error":"Invalid credentials."}), 401
    
    token = str(uuid.uuid4())
    tokens[token] = username
    return flask.jsonify({"status":"OK.", "token":token}), 200

@app.route("/delete", methods=["DELETE"])
def deleteUser():
    data = flask.request.get_json()
    username = data.get("username")
    password = data.get("password")

    if not username:
        return flask.jsonify({"error":"User does not exist."}), 400
    
    if username in deleted_users:
        return flask.jsonify({"error": "User profile is already removed."}), 409
    
    if username not in users:
        return flask.jsonify({"error":"User does not exist."}), 404
    
    token = str(uuid.uuid4())
    tokens[token] = username
    deleted_users[username] = password

    del users[username]
    return flask.jsonify({"status":"OK.", "token": token}), 200

@app.route("/updatepass", methods=["POST"])
def updatePassword():
    data = flask.request.get_json()
    username = data.get("username")
    password = data.get("password")

    if not username:
        return flask.jsonify({"error":"User does not exist."}), 400
    
    if username not in users:
        return flask.jsonify({"error":"User does not exist."}), 404
    
    users[username] = password 

    token = str(uuid.uuid4())
    tokens[token] = username
    return flask.jsonify({"status":"OK.", "token": token}), 200

@app.route("/getusers", methods=["GET"])
def getDatabase():
    for entry in users:
        print(entry)

    return flask.jsonify(users), 200

if(__name__ == "__main__"):
    app.run(port=5000, debug=True)