import flask
import pathlib

app = flask.Flask(__name__) #Create the webapp
@app.route("/hello", methods = ["GET"])
def returnMessage():
    path = pathlib.Path(__file__)
    return flask.jsonify({"message": f"Hello from {path}!",
                          "score": 42,
                          "status": "Active"})

@app.route("/submit", methods = ["POST"])
def postMessage():
    data = flask.request.get_json() # get the json body
    message = data.get("message")
    score = data.get("score")
    status = data.get("status")

    print(f"Received {message}, {score} and {status} from the backend.")
    return flask.jsonify({"message": "Request Successful!", status: "OK"})

if(__name__ == "__main__"):
    app.run(port=5000, debug=True) #run it on the port 5000