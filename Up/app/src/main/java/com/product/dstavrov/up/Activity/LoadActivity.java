package com.product.dstavrov.up.Activity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.widget.Toast;


import com.google.firebase.FirebaseApp;
import com.google.firebase.database.DataSnapshot;
import com.google.firebase.database.DatabaseError;
import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;
import com.google.firebase.database.ValueEventListener;
import com.google.firebase.messaging.FirebaseMessaging;
import com.google.gson.Gson;
import com.product.dstavrov.up.ConnectReceiver;
import com.product.dstavrov.up.Helpers.Day;
import com.product.dstavrov.up.Helpers.Group;
import com.product.dstavrov.up.Helpers.Subject;
import com.product.dstavrov.up.MyApp;
import com.product.dstavrov.up.R;
import com.product.dstavrov.up.databinding.ActivityLoadBinding;



import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;


public class LoadActivity extends AppCompatActivity implements ConnectReceiver.ConnectivityReceiverListener{

    ActivityLoadBinding mBind;
    final String TAG = "_log";
    final String KEY = "TABLE";
    final String PREFERENCES_NAME = "JSON_DATA";
    SharedPreferences shared_pref;
    private DatabaseReference mDatabase;

        public interface OnGetDataListener {
        //make new interface for call back
        void onSuccess(DataSnapshot dataSnapshot);
        void onStart();
        void onFailure();
    }
    public void readData(DatabaseReference ref, final OnGetDataListener listener) {
        listener.onStart();
        ref.addListenerForSingleValueEvent(new ValueEventListener() {
            @Override
            public void onDataChange(DataSnapshot dataSnapshot) {
                Log.d(TAG, "Load start");
                listener.onSuccess(dataSnapshot);
                Log.d(TAG, "Load start");
            }

            @Override
            public void onCancelled(DatabaseError databaseError) {
                listener.onFailure();
            }
        });

    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d(TAG, "LoadActivity start");
        mBind = DataBindingUtil.setContentView(this, R.layout.activity_load);
        FirebaseDatabase fireDatabase = FirebaseDatabase.getInstance();
        final DatabaseReference rasp_ref = fireDatabase.getReference("Группы");


        mDatabase = FirebaseDatabase.getInstance().getReference();
/*debug this proplem*/
        if(checkConnection()){
            Log.d(TAG, "connection start");

            rasp_ref.addValueEventListener(new ValueEventListener() {
                @Override
                public void onDataChange(DataSnapshot dataSnapshot) {
                    Log.d(TAG, "Load start");
                    Map<Integer, Group> rasp = new HashMap<Integer, Group>();
                    int counter = 0;
                    for(DataSnapshot group : dataSnapshot.getChildren()){
                        Group new_group = new Group();
                        new_group.setName(group.getKey());
                        Log.d(TAG, group.getKey());
                        if(group.child("Верхняя неделя") != null){
                            new_group.up_add(get_rasp(group.child("Верхняя неделя")));
                        }
                        if(group.child("Нижняя неделя") != null){
                            new_group.down_add(get_rasp(group.child("Нижняя неделя")));
                            //log_rasp(group.child("Нижняя неделя"));
                        }
                        rasp.put(counter, new_group);
                        counter++;
                        // get a group keys and inside objects
                    }
                    rasp_ref.removeEventListener(this);//delete listener
                    Log.d(TAG, "Load end");
                    Gson gson = new Gson();
                    String json = gson.toJson(rasp);


                    shared_pref = getApplicationContext().getSharedPreferences(PREFERENCES_NAME, 0);
                    SharedPreferences.Editor editor = shared_pref.edit();
                    editor.putString(KEY, json);
                    editor.apply();

                    Log.d(TAG, "json is: " + json);
                    editor.apply();//save data!
                    go_main();//load is complete go to main activity
                }

                @Override
                public void onCancelled(DatabaseError databaseError) {
                    Log.d(TAG, "database error!!!- " + databaseError.getMessage());
                }
            });
            Log.d(TAG, "exit line");
        }else {
            Toast t = Toast.makeText(getApplicationContext(), "No Internet Connection", Toast.LENGTH_SHORT);
            t.show();
            go_main();

        }

    }

    private void go_main(){
        Intent intent = new Intent(this, MainActivity.class);
        startActivity(intent);
    }

        private boolean checkConnection() {
            return ConnectReceiver.isConnected();
        }
        @Override
        protected void onResume() {
            super.onResume();
            // register connection status listener
            MyApp.getInstance().setConnectivityListener(this);
        }

    private ArrayList<Day> get_rasp (DataSnapshot child){
        ArrayList<Day> days = new ArrayList<>();
        for(DataSnapshot day: child.getChildren()){
            Day new_day = new Day();
            new_day.setName(day.getKey());
            for(DataSnapshot time: day.getChildren()){
                Subject subject = new Subject();
                subject.setName(time.child("name").getValue(String.class));
                subject.setAud(time.child("aud").getValue(String.class));
                subject.setLector(time.child("lector").getValue(String.class));
                new_day.add_subject(time.getKey(), subject);
            }
            days.add(new_day);
        }
        return days;
    }



    @Override
    public void onNetworkConnectionChanged(boolean isConnected) {
        Log.d(TAG, "Connect is Fail");
    }

    private void log_rasp(DataSnapshot child){
        for(DataSnapshot day: child.getChildren()){
            Log.d(TAG, "\nday: " + day.getKey());
            for(DataSnapshot time: day.getChildren()){
                Log.d(TAG, "\n    name: "+ time.child("name").getValue(String.class) +
                        "\n    aud: " + time.child("aud").getValue(String.class)  +
                        "\n    lector: " +  time.child("lector").getValue(String.class)+
                        "\n    time: " + time.getKey());
            }
        }
    }
}
