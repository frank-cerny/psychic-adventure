<?php

/**
 * Plugin Name: ColibriWP Fix Options
 * 
 * How to use:
 *
 * 1. Put this file inside the wp-content/plugins/ directory;
 * 2. In the Wordpress Dashboard, navigate to the Extensions page;
 * 3. Enable the "ColibriWP Fix Options" extension;
 * 4. Load any page with a `colibriwp-try-fix-serialization` GET parameter.
 *    For instance, `https://www.your-wordpress-site.com/index.php?colibriwp-try-fix-serialization=true`
 */

function fix_str_length($matches) {
    $string = $matches[2];
    $right_length = strlen($string); // yes, strlen even for UTF-8 characters, PHP wants the mem size, not the char count
    return 's:' . $right_length . ':"' . $string . '";';
}

function extendthemes_fix_serialized($string)
{
    // securities
    if (!preg_match('/^[aOs]:/', $string)) return $string;
    if (@unserialize($string) !== false) return $string;
    $string = preg_replace("%\n%", "", $string);
    // doublequote exploding
    $data = preg_replace('%";%', "µµµ", $string);
    $tab = explode("µµµ", $data);
    $new_data = '';
    foreach ($tab as $line) {
        $new_data .= preg_replace_callback('%\bs:(\d+):"(.*)%', 'fix_str_length', $line);
    }
    return $new_data;
}

function get_option_raw($option) {
	global $wpdb;
        $suppress = $wpdb->suppress_errors();
        $row      = $wpdb->get_row( $wpdb->prepare( "SELECT option_value FROM $wpdb->options WHERE option_name = %s LIMIT 1", $option ) );
        $wpdb->suppress_errors( $suppress );
	return $row->option_value;
}

add_action('init', function () {
    if (isset($_REQUEST['colibriwp-try-fix-serialization'])) {
        $x = get_option_raw( 'extend_builder_theme');
        if (is_string($x)) {
            $x = preg_replace('#\r?\n#', "  ", $x);
            $x = extendthemes_fix_serialized($x);

            $y = unserialize($x);

            if(is_array($y)){
                update_option('extend_builder_theme', $y);
                var_dump($y);
            } else {
                wp_die( 'Au au' );
            }
            
        }
    }
});