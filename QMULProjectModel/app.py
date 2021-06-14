import json, io
import urllib.request, urllib.response
import flask
from flask import Flask, request, jsonify, flash, redirect, url_for, json
from flask.templating import render_template
import base64
app = Flask(__name__, template_folder='Template')

ALLOWED_EXTENSIONS = {'png', 'jpg', 'jpeg', 'gif'}

def allowed_file(filename):
    return '.' in filename and \
           filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS

@app.route('/', methods=['GET', 'POST'])
def uploadFile():
    if flask.request.method == 'GET':
        # Just render the initial form, to get input
        return(flask.render_template('index.html')), 200
    
    if request.method == 'POST':
        # check if the post request has the file part
        if request.files['file'].filename == '':
            result = 'No file selected'
            return flask.redirect(url_for('viewBase64', encstring = json.dumps(result)))
        file = request.files['file']
        if file and allowed_file(file.filename):
            encoded_string = base64.b64encode(file.read())
            result = encoded_string.decode('utf-8')
        else:
            result = 'Unsupported file format'
        encstring = json.dumps(result)
        return flask.redirect(url_for('viewBase64', encstring = encstring))
    
    print("debug check")

@app.route('/viewBase64/', methods=['GET', 'POST'])
def viewBase64():
    if flask.request.method == 'GET':
        # Just render the initial form, to get input
        x = request.args.get('encstring')
        return jsonify(x)
    

if __name__ == '__main__':
    # Threaded option to enable multiple instances for multiple user access support
    app.run(threaded=True, port=5000, debug=True)