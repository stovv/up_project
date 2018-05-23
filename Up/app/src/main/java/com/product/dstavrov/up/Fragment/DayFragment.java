package com.product.dstavrov.up.Fragment;


import android.content.res.AssetManager;
import android.graphics.Color;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.view.ViewPager;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.Spinner;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.Toast;

import com.google.firebase.database.DataSnapshot;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.product.dstavrov.up.Activity.MainActivity;
import com.product.dstavrov.up.Adapter.CardPagerAdapter;
import com.product.dstavrov.up.Helpers.Day;
import com.product.dstavrov.up.Helpers.Group;
import com.product.dstavrov.up.Helpers.ShadowTransformer;
import com.product.dstavrov.up.Helpers.Subject;
import com.product.dstavrov.up.Item.CardItem;
import com.product.dstavrov.up.Item.GroupItem;
import com.product.dstavrov.up.Item.SubjectItem;
import com.product.dstavrov.up.R;
import com.toptoche.searchablespinnerlibrary.SearchableSpinner;

import java.io.IOException;
import java.io.InputStream;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;


public class DayFragment extends Fragment {

    ViewPager mViewPager;

    final  String TAG = "DayFragment";
    private CardPagerAdapter mCardAdapter;
    private ShadowTransformer mCardShadowTransformer;
    private String json = "";
    private Map<Integer, Group> rasp;
    private String primary_Group = "";


    public DayFragment() {

    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        Bundle bundle = this.getArguments();
        if (bundle != null) {
            json = bundle.getString("JSON", "");
            if(!json.equals("")){
                Log.d(TAG, "JSON in Day is not null \n" + "[ "+ json +" ]");
                Gson gson = new Gson();
                Type type = new TypeToken<Map<Integer, Group>>(){}.getType();
                rasp = gson.fromJson(json, type);
            }else {
                Log.d(TAG, "JSON in Day null \n" + "[ "+ json +" ]");
            }
        }else{
            Log.d(TAG, "Bundle is null \n" + "[ "+ json +" ]");
        }

        return inflater.inflate(R.layout.fragment_day, container, false);
    }

    @Override
    public void onViewCreated(View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);



        Calendar calendar = Calendar.getInstance();
        String strDate = calendar.get(Calendar.DATE) + " " +calendar.get(Calendar.MONTH) + " " + calendar.get(Calendar.YEAR);
        TextView date = (TextView) getActivity().findViewById(R.id.date);
        date.setText(String.format("%1$td %1$tB %1$tY", calendar));

 /*       mCardAdapter = new CardPagerAdapter();

        mCardShadowTransformer = new ShadowTransformer(mViewPager, mCardAdapter);
        mViewPager.setAdapter(mCardAdapter);
        mViewPager.setPageTransformer(false, mCardShadowTransformer);
        mViewPager.setOffscreenPageLimit(3);
        mCardShadowTransformer.enableScaling(true);*/

        final Map<String, Map<String, List<CardItem>>> cards = get_groups_card(rasp);
        Spinner spinner = (Spinner) getActivity().findViewById(R.id.spinner);
        //spinner.setTitle("Select Group");
        List<String> group = new ArrayList<>(cards.keySet());
       // ArrayAdapter<CharSequence> adapter_group = new ArrayAdapter<CharSequence>(getActivity().getApplication(), R.layout.activity_main, group);
        ArrayAdapter<String> adapter_group = new ArrayAdapter<String>(getActivity(),
                R.layout.group_style, group);
        //adapter_group.setDropDownViewResource(R.style.spinnerDropDownItemStyle);
        spinner.setAdapter(adapter_group);


        mViewPager = (ViewPager) getActivity().findViewById(R.id.viewPager);
        mCardAdapter = new CardPagerAdapter();
        for(CardItem cardItem : cards.get(spinner.getSelectedItem().toString()).get("up")){
            mCardAdapter.addCardItem(cardItem);
        }
        mCardShadowTransformer = new ShadowTransformer(mViewPager, mCardAdapter);
        mViewPager.setAdapter(mCardAdapter);
        mViewPager.setPageTransformer(false, mCardShadowTransformer);
        mViewPager.setOffscreenPageLimit(3);
        mCardShadowTransformer.enableScaling(false);

        SearchableSpinner spin = getActivity().findViewById(R.id.spinner);
        spin.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                CardPagerAdapter newCardAdapter = new CardPagerAdapter();
                for(CardItem cardItem : cards.get(parent.getItemAtPosition(position).toString()).get("up")){
                    newCardAdapter.addCardItem(cardItem);
                }
                mViewPager.setAdapter(newCardAdapter);
                primary_Group = parent.getItemAtPosition(position).toString();
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });
/*        spinner.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {

            }
        });*/
        final Switch switcher = getActivity().findViewById(R.id.ud_switcher);
        switcher.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if(switcher.isChecked()){
                    switcher.setText("down week");
                    CardPagerAdapter newCardAdapter = new CardPagerAdapter();
                    for(CardItem cardItem : cards.get(primary_Group).get("down")){
                        newCardAdapter.addCardItem(cardItem);
                    }
                    mViewPager.setAdapter(newCardAdapter);
                }else{
                    CardPagerAdapter newCardAdapter = new CardPagerAdapter();
                    for(CardItem cardItem : cards.get(primary_Group).get("up")){
                        newCardAdapter.addCardItem(cardItem);
                    }
                    mViewPager.setAdapter(newCardAdapter);
                }
            }
        });
    }

    private Map<String, Map<String, List<CardItem>>> get_groups_card(Map<Integer, Group> scheduel){

        Map<String, Map<String, List<CardItem>>> groups_day = new HashMap<>();
        for (Group group : scheduel.values()) {
            //List<CardItem> cards = new ArrayList<>();
            //Log.d(TAG, "Group name: " + group.getName());
            Map<String, List<CardItem>> up_down = new HashMap<String, List<CardItem>>();
            List<CardItem> cards_up = new ArrayList<>();
            for (int i = 0; i < group.getUp_rasp().size(); i++) {
                //Log.d(TAG, "   Day name: " + group.getUp_rasp().get(i).getName());

                List<SubjectItem> subjects = new ArrayList<>();
                Day d = group.getUp_rasp().get(i);

                for(String time : d.getRasp().keySet()){
                    Subject subject = d.getRasp().get(time);
                    String time_format;
                    int index = time.indexOf("-");
                    time_format = time.substring(0,index) + "\n" + time.substring(index+1);
                    // Log.d(TAG,time_format);
                    subjects.add(new SubjectItem(time_format,subject, getResources().getColor(R.color.primaryLightColor)));
                }
                cards_up.add(new CardItem(d.getName(),subjects));

                if(primary_Group.isEmpty()) primary_Group = group.getName();
            }
            up_down.put("up", cards_up);

            List<CardItem> cards_down = new ArrayList<>();
            for (int i = 0; i < group.getDown_rasp().size(); i++) {
                //Log.d(TAG, "   Day name: " + group.getUp_rasp().get(i).getName());
                List<CardItem> cards = new ArrayList<>();
                List<SubjectItem> subjects = new ArrayList<>();
                Day d = group.getDown_rasp().get(i);

                for(String time : d.getRasp().keySet()){
                    Subject subject = d.getRasp().get(time);
                    String time_format;
                    int index = time.indexOf("-");
                    time_format = time.substring(0,index) + "\n" + time.substring(index+1);
                    // Log.d(TAG,time_format);
                    subjects.add(new SubjectItem(time_format,subject, getResources().getColor(R.color.primaryLightColor)));
                }
                cards_down.add(new CardItem(d.getName(),subjects));

                if(primary_Group.isEmpty()) primary_Group = group.getName();
            }
            up_down.put("down", cards_down);
            groups_day.put(group.getName(), up_down);

        }

        return groups_day;
    }



}
