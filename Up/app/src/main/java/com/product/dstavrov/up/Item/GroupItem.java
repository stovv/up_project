package com.product.dstavrov.up.Item;

import android.view.View;

/**
 * Created by vicru on 28.03.2018.
 */

public class GroupItem {
    private String name;
    private String num;
    private int color;
    private View.OnClickListener listener;

    public GroupItem(){}
    public GroupItem(String name, String num, int color, View.OnClickListener listener){
        this.name = name;
        this.num = num;
        this.color = color;
        this.listener = listener;
    }

    public String getName() {
        return name;
    }

    public String getNum() {
        return num;
    }
    public int getColor(){
        return color;
    }
    public View.OnClickListener getOnclickListner(){
        return listener;
    }
}
