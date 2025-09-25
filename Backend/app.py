import flask
import pathlib

app = flask.Flask(__name__) #Create the webapp
@app.route("/hello", methods = ["GET"])
def returnMessage():
    path = pathlib.Path(__file__)
    return flask.jsonify({"message": f"Hello from {path}!",
                          "score": 42,
                          "status": "Active"})

@app.route("/post", methods = ["POST"])
def postMessage():
    try:
        data = flask.request.get_json(force=True) # get the json body, even if the content header is missing or not
    except Exception as e:
        return flask.jsonify({"error":"Bad request. Unavailable to load the body."}), 400
    
    message = data.get("message")
    score = data.get("score")
    status = data.get("status")

    if((message is None) or (score is None)):
        return flask.jsonify({"error":"Invalid values for 'message' or 'score'."}), 422
    
    if(not isinstance(score, int) or score < 0):
        return flask.jsonify({"error": "'score' must be a positive integer."}), 422

    print(f"Received {message}, {score} and {status} from the backend.")
    return flask.jsonify(
        {
            "message": message,
            "score": score,
            "status": status
        }
    )

if(__name__ == "__main__"):
    app.run(port=5000, debug=True) #run it on the port 5000