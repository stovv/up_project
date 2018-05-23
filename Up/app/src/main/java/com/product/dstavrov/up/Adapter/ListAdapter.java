package com.product.dstavrov.up.Adapter;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.product.dstavrov.up.Item.GroupItem;
import com.product.dstavrov.up.R;

import java.util.ArrayList;
import java.util.List;


/**
 * Created by vicru on 28.03.2018.
 */

public class ListAdapter extends RecyclerView.Adapter<ListAdapter.ViewHolder> {

    List<GroupItem> items = new ArrayList();
    public static class ViewHolder extends RecyclerView.ViewHolder {
        public TextView name;
        public TextView num;
        public LinearLayout backg;
        private RelativeLayout trnd_background;
        public ViewHolder(View v) {
            super(v);
            name = v.findViewById(R.id.trend_name);
            num = v.findViewById(R.id.group_num);
            backg = v.findViewById(R.id.text_bckg_list);
            trnd_background = v.findViewById(R.id.background_trnd);
        }
    }

    public ListAdapter(List<GroupItem> items) {
        this.items = items;
    }
    @Override
    public ListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.adapter_list, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public int getItemCount() {
        return items.size();
    }

    @Override
    public void onBindViewHolder(ViewHolder holder, int position) {
        GroupItem m = items.get(position);
        holder.name.setText(m.getName());
        holder.num.setText(m.getNum());
        holder.backg.setOnClickListener(m.getOnclickListner());
        holder.trnd_background.setBackgroundColor(m.getColor());
    }
}

