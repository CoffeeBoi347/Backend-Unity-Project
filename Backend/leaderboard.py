import flask

app = flask.Flask(__name__)
leaderboard = []

@app.route("/submit", methods = ["POST"])
def submit_score():
    data = flask.request.get_json()
    name = data.get("name")
    score = data.get("score")

    if not name or score is None:
        return flask.jsonify({"error": "Invalid request. Missing 'player' or 'score'."}), 400
    
    leaderboard.append({"name": name, "score": score})
    leaderboard.sort(key=lambda x:x["score"], reverse=True)

    return flask.jsonify({"status": "OK. message submitted to server."}), 200

@app.route("/leaderboard", methods = ["GET"])
def get_leaderboard():
    top = leaderboard[:5]
    if not top:
        return flask.jsonify({"error": "Leaderboard not found."}), 404
    
    for value in top:
        print(value)

    return flask.jsonify({f"message": "OK. Value retrieved successfully.", "leaderboard": top}), 200


if(__name__ == "__main__"):
    app.run(port=5000, debug=True) #run it on the port 5000