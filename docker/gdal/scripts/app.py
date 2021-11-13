# app.py - this should run automativly in when the docker is built and run
print ('info - running script in gdal container \n'*100)
import subprocess
import sys
import os

# check if we have args in the python call eg : app.py path_to_shp_folder path_to_json_folder
if len(sys.argv)==2 : 
    # no paths found so we are going to set the default path of the docker \tmp
    print ('arguments where not found: (path to shape file or path to json file)')
    #sys.exit()
    
else:
    # for debug
    path_to_shp_folder = r"./from_shapefile"   
    path_to_json_folder = r"./to_GeoJSON"
        

# find the *.shp file
for f in os.listdir(path_to_shp_folder) :
    print (f)
    if f.split('.')[-1] == 'shp':
        path_to_shp_file = path_to_shp_folder + "//" + f
        # the name of the json file is the same name as the shp file
        path_to_json_file = path_to_json_folder + "//" + f.split('.')[0] + '.json' 
        cmd = 'ogr2ogr -f GeoJSON '
        cmd+= str(path_to_json_file)
        cmd+= ' '
        cmd+=str(path_to_shp_file)
        print ('info - runing: ', cmd)
        subprocess.run(cmd)
        print ('info - results: ', os.listdir(path_to_json_folder))
        
