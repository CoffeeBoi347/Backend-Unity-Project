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
    data = flask.request.get_json()
    message = data.GET("message")
    status = data.GET("status")
    print(f"Received {message} and {status} from the backend.")
    return flask.jsonify({"message": "Request Successful!", status: "OK"})

if(__name__ == "__main__"):
    app.run(port=5000, debug=True)