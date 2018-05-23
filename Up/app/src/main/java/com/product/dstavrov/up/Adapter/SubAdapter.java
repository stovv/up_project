package com.product.dstavrov.up.Adapter;

import android.graphics.Color;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.product.dstavrov.up.Item.SubjectItem;
import com.product.dstavrov.up.R;

import java.util.List;

/**
 * Created by vicru on 26.03.2018.
 */


public class SubAdapter extends RecyclerView.Adapter<SubAdapter.ViewHolder> {
    private List<SubjectItem> items;

    public static class ViewHolder extends RecyclerView.ViewHolder {
        public TextView time;
        public TextView sub_name;
        public TextView prof_name;
        public TextView aud_num;
        private RelativeLayout backg;
        private LinearLayout text_bckg;
        public ViewHolder(View v) {
            super(v);

            time = v.findViewById(R.id.time);
            sub_name = v.findViewById(R.id.sub_name);
            prof_name = v.findViewById(R.id.prof_name);
            aud_num = v.findViewById(R.id.aud_name);
            backg = v.findViewById(R.id.background_time);
            text_bckg = v.findViewById(R.id.text_bckg);
        }
    }

    public SubAdapter(List<SubjectItem> items) {
        this.items = items;
    }

    @Override
    public SubAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.adapter_sub, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public int getItemCount() {
        return items.size();
    }

    @Override
    public void onBindViewHolder(ViewHolder holder, int position) {
        SubjectItem m = items.get(position);
        holder.time.setText(m.getTime());
        holder.backg.setBackgroundColor(m.getColor());
        if(m.getColor() == Color.parseColor("#320b86")){
            holder.text_bckg.setBackgroundColor(Color.parseColor("#a2a2a3"));
            holder.sub_name.setTextColor(Color.WHITE);
            holder.prof_name.setTextColor(Color.WHITE);
            holder.aud_num.setTextColor(Color.WHITE);
        }
        holder.sub_name.setText(m.getName_of_subject());
        holder.prof_name.setText(m.getName_of_prof());
        holder.aud_num.setText(m.getAud_num());
    }

}