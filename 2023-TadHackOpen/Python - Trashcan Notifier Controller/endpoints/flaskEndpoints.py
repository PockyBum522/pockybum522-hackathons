from __main__ import app


@app.route('/detect/<accelerometer_x_gforce>/<accelerometer_y_gforce>/<accelerometer_z_gforce>', methods=['GET'])
def accelerometer_detect_change(accelerometer_x_gforce, accelerometer_y_gforce, accelerometer_z_gforce):

    print(f'x: {accelerometer_x_gforce}, y: {accelerometer_y_gforce}, z: {accelerometer_z_gforce}')

    return "Data received successfully"
