package com.product.dstavrov.up.Activity;


import android.content.SharedPreferences;
import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.FragmentManager;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.widget.Toast;

import com.google.firebase.FirebaseApp;
import com.google.firebase.database.DataSnapshot;
import com.google.firebase.messaging.FirebaseMessaging;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.product.dstavrov.up.Adapter.CardPagerAdapter;
import com.product.dstavrov.up.Fragment.DayFragment;
import com.product.dstavrov.up.Helpers.Day;
import com.product.dstavrov.up.Helpers.Group;
import com.product.dstavrov.up.Helpers.ShadowTransformer;
import com.product.dstavrov.up.Helpers.Subject;
import com.product.dstavrov.up.Item.CardItem;
import com.product.dstavrov.up.Item.SubjectItem;
import com.product.dstavrov.up.R;
import com.product.dstavrov.up.databinding.ActivityMainBinding;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Map;


public class MainActivity extends AppCompatActivity {
    ActivityMainBinding mBind;
    final String TAG = MainActivity.class.getSimpleName();
    final String KEY = "TABLE";
    final String PREFERENCES_NAME = "JSON_DATA";
    DayFragment dayFragment;
    //ListTable listTable;



    SharedPreferences shared_pref;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        FirebaseMessaging.getInstance().subscribeToTopic("FireNotify");//subscribe to notify
        mBind = DataBindingUtil.setContentView(this, R.layout.activity_main);

        shared_pref = getApplicationContext().getSharedPreferences(PREFERENCES_NAME, 0);
        String json = shared_pref.getString(KEY, "");
        if(!json.equals("")){
            //Log.d(TAG, "JSON is not null \n" + "[ "+ json +" ]");
/*            Gson gson = new Gson();
            Type type = new TypeToken<Map<Integer, Group>>(){}.getType();
            Map<Integer, Group> rasp = gson.fromJson(json, type);
*/
            dayFragment = new DayFragment();
            Bundle bundle = new Bundle();
            bundle.putString("JSON", json);

            dayFragment.setArguments(bundle);

            FragmentManager fragmentManager = getSupportFragmentManager();
            fragmentManager.beginTransaction()
                    .replace(R.id.fragments, dayFragment, "DAY")
                    .commit();

        }else{
            Toast.makeText(getApplicationContext(), "No Saved Table!\nPlease get Internet Connection and\n restart app", Toast.LENGTH_SHORT).show();
        }
    }

    @Override
    public void onBackPressed() {

    }

}

